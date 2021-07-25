using AuthorizationService.Dto;
using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class RefreshTokensInSqlRepository : IRefreshTokens
    {
        public Func<DateTime> GetDtFunc { get; set; } = () => DateTime.Now;
        private readonly AuthorizationDbContext _db;
        private readonly ILogger<RefreshTokensInSqlRepository> _logger;

        public RefreshTokensInSqlRepository(AuthorizationDbContext db,
            ILogger<RefreshTokensInSqlRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RefreshToken> CreateRefreshToken(Account account, int expiresSec)
        {
            if (expiresSec <= 0) throw new ArgumentOutOfRangeException(nameof(expiresSec));

            var currentDt = GetDtFunc();

            var entity = new RefreshToken(account.AccountId, currentDt, expiresSec);

            await _db.RefreshTokens.AddAsync(entity);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            //var deleted = col.DeleteMany(c => c.AccountId == account.Id && c.ExpiresDt < currentDt); ??

            return entity;
        }
        public async Task<RefreshToken> ReCreateRefreshToken(Guid previousRefreshId, int expiresSec)
        {

            var prevRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.RefreshTokenId == previousRefreshId);

            if (prevRefreshToken == null) return null;

            var currentDt = GetDtFunc();

            if (prevRefreshToken.ExpiresDate < currentDt) //Истек
            {
                _db.RefreshTokens.Remove(prevRefreshToken);
                await _db.SaveChangesAsync();
                await _db.DisposeAsync();
                return null;
            }
            var entity = new RefreshToken(prevRefreshToken.AccountId, currentDt, expiresSec);

            await _db.RefreshTokens.AddAsync(entity);
            _db.RefreshTokens.Remove(prevRefreshToken);
            await _db.SaveChangesAsync();
           // await _db.DisposeAsync();

            return entity;
        }

        public async Task<bool> DeleteRefreshToken(Guid id)
        {
            var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.RefreshTokenId == id);

            if (refreshToken == null) return false;

            _db.RefreshTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<bool> DeleteRefreshTokensForAccount(Guid accountId)
        {
            var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.AccountId == accountId);

            if (refreshToken == null) return false;

            _db.RefreshTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<List<RefreshTokenDto>> GetAllRefreshTokens()
        {

            var refreshTokens = await _db.RefreshTokens.ToListAsync();

            List<RefreshTokenDto> refreshTokensDto = new List<RefreshTokenDto>();

            foreach (var refreshToken in refreshTokens)
            {
                refreshTokensDto.Add(new RefreshTokenDto(refreshToken));
            }

            return refreshTokensDto;
        }

        public async Task<List<RefreshTokenDto>> GetAllRefreshTokens(Guid accountId)
        {
            var refreshTokens = await _db.RefreshTokens.Where(r => r.AccountId == accountId).ToListAsync();

            List<RefreshTokenDto> refreshTokensDto = new List<RefreshTokenDto>();

            foreach (var refreshToken in refreshTokens)
            {
                refreshTokensDto.Add(new RefreshTokenDto(refreshToken));
            }

            return refreshTokensDto;
        }
    }
}
