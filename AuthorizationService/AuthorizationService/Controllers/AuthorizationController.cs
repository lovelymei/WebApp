using AuthorizationService.Certificates;
using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    [Route("identity/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        public Func<DateTime> GetCurrentDtFunc = () => DateTime.Now;
        private readonly IConfiguration _config;
        private readonly IAccounts _accounts;
        private readonly IRefreshTokens _refreshTokens;

        public AuthorizationController(IAccounts accounts,
            IConfiguration config,
            IRefreshTokens refreshTokens)
        {
            _refreshTokens = refreshTokens;
            _accounts = accounts;
            _config = config;
        }

        /// <summary>
        /// Создание JWT
        /// </summary>
        /// <response code="401">Не верные логин/пароль</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDto>> CreateToken([FromBody] SignIn signIn)
        {
            var account = await _accounts.Authenticate(signIn.Email, signIn.Password);

            if (account == null) return Unauthorized();

            var expiresSec = int.Parse(_config["Jwt:ExpiresSec"]);

            var refresh = await _refreshTokens.CreateRefreshToken(account, 864000); //TODO В конфиг

            var token = await BuildToken(new AccountDtoForAuthorization(account), refresh.RefreshTokenId, expiresSec);

            return Ok(token);
        }

        /// <summary>
        /// Обновление JWT
        /// </summary>
        /// <response code="401">Токен просрочен. Вход по логину и паролю (/api/Token/signin)</response>
        /// <response code="403">Аккаунт деактивирован</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("refreshId={id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TokenDto>> RefreshToken(Guid id)
        {
            //var tokenIp = HttpContext.User.GetAccountLastIp();
            //var currentIp = HttpContext.Connection.RemoteIpAddress.ToString();

            //if (tokenIp != HttpContext.Connection.RemoteIpAddress.ToString())
            //{
            //    _log.Warn($"Client ip address does not match the address in the JWT. jwt ip=[{tokenIp}] current=[{currentIp}]");
            //}

            var expiresSec = int.Parse(_config["Jwt:ExpiresSec"]);
            var newRefreshToken = await _refreshTokens.ReCreateRefreshToken(id, 864000); //TODO В конфиг
            if (newRefreshToken == null) return Unauthorized();

            var account = await _accounts.GetAccount(newRefreshToken.AccountId);
            if (account == null) return Forbid();

            var token = await BuildToken(new AccountDtoForAuthorization(account), newRefreshToken.RefreshTokenId, expiresSec);
            return Ok(token);
        }

        /// <summary>
        /// Получить список всех RefreshToken 
        /// </summary>
        /// <returns></returns>
        /// <response code = "204" > Список RefreshToken пуст</response>

        [HttpGet]
        [AuthorizeEnum(Roles.administratior)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[AuthorizeEnum(Roles.Administrator, Roles.SuperAdministrator)]
        public async Task<ActionResult<RefreshToken[]>> GetAll()
        {
            var tokens = await _refreshTokens.GetAllRefreshTokens();
            //if (tokens.Count == 0) return NoContent();
            return Ok(tokens);
        }

        /// <summary>
        /// Получить список всех RefreshToken для аккаунта
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Список RefreshToken пуст</response>
        [HttpGet("accountId={accountId}")]
        [AuthorizeEnum(Roles.administratior)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[AuthorizeEnum(Roles.Administrator, Roles.SuperAdministrator)]
        public async Task<ActionResult<RefreshToken[]>> GetAll(Guid accountId)
        {
            var tokens = await _refreshTokens.GetAllRefreshTokens(accountId);
            //if (tokens.Count == 0) return NoContent();
            return Ok(tokens);
        }

        /// <summary>
        /// Удалить RefreshToken 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("tokenId={tokenId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizeEnum(Roles.administratior)]
        public async Task<ActionResult<RefreshToken[]>> DeleteToken(Guid tokenId)
        {
            bool isDeleted = await _refreshTokens.DeleteRefreshToken(tokenId);
            return isDeleted ? Ok() : NoContent();
        }

        /// <summary>
        /// Удалить RefreshToken для аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpDelete("accountId={accountId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizeEnum(Roles.administratior)]
        public ActionResult<RefreshToken[]> DeleteTokensForAccount(Guid accountId)
        {
            //_refreshTokens.DeleteRefreshTokensForAccount(accountId);
            return Ok();
        }

        private async Task<TokenDto> BuildToken(AccountDtoForAuthorization account, Guid refreshId, int expiresSec)
        {
            //время создания токена
            var expiresDt = GetCurrentDtFunc.Invoke().AddSeconds(expiresSec);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.NickName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.RoleName.ToString()),
                new Claim(ClaimTypes.PrimarySid, account.Id.ToString()),
            };

            SigningAudienceCertificate signingAudienceCertificate = new SigningAudienceCertificate(_config);
            var creds = await signingAudienceCertificate.GetAudienceSigningKey();

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expiresDt,
                signingCredentials: creds);

            return new TokenDto()
            {
                Expires = expiresDt,
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                Account = account,
                RefreshTokenId = refreshId
            };
        }
    }
}
