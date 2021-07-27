using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AccountController : Controller
    {
        private readonly AuthorizationDbContext _db;
        private readonly IAccounts _accounts;

        public AccountController(IAccounts accounts, AuthorizationDbContext db)
        {
            _db = db;
            _accounts = accounts;
        }

        /// <summary>
        /// Получить все аккаунты
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        //[AuthorizeEnum(Roles.administratior)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _accounts.GetAllAccounts();
            if (accounts == null) return NotFound();
            return Ok(accounts);
        }

        /// <summary>
        /// Получить все удаленные аккаунты
        /// </summary>
        /// <returns></returns>
        [HttpGet("allDeleted")]
        [AuthorizeEnum(Roles.administratior)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountDto>>> GetAllDeletedAccounts()
        {
            var deletedAccounts = await _accounts.GetAllDeletedAccounts();
            return Ok(deletedAccounts);
        }


        /// <summary>
        /// Получить аккаунт по id
        /// </summary>
        /// <param name = "id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetCurrentAccount([Required] Guid id)
        {
            var account = await _accounts.GetAccount(id);
            if (account == null) return NotFound();
            return new AccountDto(account);
        }


        /// <summary>
        /// Удалить аккаунт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccount([Required] Guid id)
        {
            var isDeleted = await _accounts.DeleteAccount(id);
            return isDeleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Создать аккаунт для слушателя
        /// </summary>
        /// <param name="listenerCreateDto"> Данные слушателя </param>
        /// <returns></returns>
        [HttpPost("listener")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDto>> RegisterListenerAccount([FromBody] AccountCreateDto listenerCreateDto)
        {
            var createdListener = await _accounts.RegisterListenerAccount(listenerCreateDto);
            return createdListener;
        }

        /// <summary>
        /// Создать аккаунт для исполнителя
        /// </summary>
        /// <param name="performerCreateDto"> Данные исполнителя </param>
        /// <returns></returns>
        [HttpPost("performer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDto>> RegisterPerformerAccount([FromBody] AccountCreateDto performerCreateDto)
        {
            var createdPerformer = await _accounts.RegisterPerformerAccount(performerCreateDto);
            return createdPerformer;
        }


        /// <summary>
        /// Создать аккаунт для админа
        /// </summary>
        /// <param name="adminCreateDto"> Данные исполнителя </param>
        /// <returns></returns>
        [HttpPost("admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDto>> RegisterAdminAccount([FromBody] AccountCreateDto adminCreateDto)
        {
            var createdPerformer = await _accounts.RegisterAdminAccount(adminCreateDto);
            return createdPerformer;
        }


        /// <summary>
        /// Обновить аккаунт
        /// </summary>
        /// <param name="id"> Идентификатор</param>
        /// <param name="accounCreateDto"> Данные для обновления </param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateAccount([Required] Guid id, [FromBody] AccountCreateDto accounCreateDto)
        {
            var isUpdated = await _accounts.UpdateAccount(id, accounCreateDto);

            return isUpdated ? Ok() : NotFound();
        }

        /// <summary>
        /// Восстановить аккаунт
        /// </summary>
        /// <param name="deletedAccountId "> Идентификатор </param>
        /// <returns></returns>
        [HttpPost("{deletedAccountId}")]
        [AuthorizeEnum(Roles.administratior)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> RestoreAccount([Required] Guid deletedAccountId)
        {
            var isRestored = await _accounts.RestoreAccount(deletedAccountId);

            return Ok(isRestored);
        }

    }
}