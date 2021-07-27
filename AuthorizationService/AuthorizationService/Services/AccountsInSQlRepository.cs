using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class AccountsInSQlRepository : IAccounts
    {
        private readonly AuthorizationDbContext _db;
        private readonly ILogger<AccountsInSQlRepository> _logger;

        public AccountsInSQlRepository(AuthorizationDbContext db,
            ILogger<AccountsInSQlRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            var accounts = await _db.Accounts.Where(c => c.IsDeleted == false).ToListAsync();

            List<AccountDto> accountsDto = new List<AccountDto>();

            foreach (var account in accounts)
            {
                accountsDto.Add(new AccountDto(account));
            }

            return accountsDto;
        }


        public async Task<Account> GetAccount(Guid id)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.AccountId == id && c.IsDeleted == false);

            if (account == null) return null;

            return account;
        }


        private async Task<AccountDto> CreateAccount(AccountCreateDto accountCreateDto, Roles role)
        {
            var salt = GenerateSalt();
            var enteredPassHash = accountCreateDto.Password.ToPasswordHash(salt);

            Login newLoginModel = new Login()
            {
                Email = accountCreateDto.Email,
                Salt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(enteredPassHash),
            };

            Account account = new Account()
            {
                Login = newLoginModel,
                NickName = accountCreateDto.NickName,
                Role = role
            };

            await _db.Accounts.AddAsync(account);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return new AccountDto(account);

        }

        public async Task<AccountDto> RegisterListenerAccount(AccountCreateDto accountCreateDto)
        {
            var listenerAccontDto = await CreateAccount(accountCreateDto, Roles.listener);
            return listenerAccontDto;
        }

        public async Task<AccountDto> RegisterPerformerAccount(AccountCreateDto accountCreateDto)
        {
            var performerAccountDto = await CreateAccount(accountCreateDto, Roles.performer);
            return performerAccountDto;
        }

        public async Task<AccountDto> RegisterAdminAccount(AccountCreateDto accountCreateDto)
        {
            var adminAccountDto = await CreateAccount(accountCreateDto, Roles.administratior);
            return adminAccountDto;
        }


        public async Task<bool> UpdateAccount(Guid id, AccountCreateDto accountCreateDto)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.AccountId == id);
            var login = await _db.Logins.FirstOrDefaultAsync(c => c.AccountId == id);


            if (account == null || login == null) return false;

            var salt = GenerateSalt();
            var enteredPassHash = accountCreateDto.Password.ToPasswordHash(salt);

            login.Email = accountCreateDto.Email;
            login.Salt = Convert.ToBase64String(salt);
            login.PasswordHash = Convert.ToBase64String(enteredPassHash);

            account.NickName = accountCreateDto.NickName;

            _db.Accounts.Update(account);
            _db.Logins.Update(login);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> DeleteAccount(Guid id)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.AccountId == id);

            if (account == null) return false;

            account.IsDeleted = true;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<List<AccountDto>> GetAllDeletedAccounts()
        {
            var deletedAccounts = await _db.Accounts.Where(c => c.IsDeleted == true).ToListAsync();

            List<AccountDto> accountsDto = new List<AccountDto>();

            foreach (var account in deletedAccounts)
            {
                accountsDto.Add(new AccountDto(account));
            }

            return accountsDto;
        }

        public async Task<bool> RestoreAccount(Guid id)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(l => l.AccountId == id);

            if (account == null) return false;

            account.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        private static byte[] GenerateSalt()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[16];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        public async Task<Account> Authenticate(string email, string password)
        {
            var login = await _db.Logins.FirstOrDefaultAsync(c => c.Email == email);

            if (login == null) return null;

            var enteredPassHash = password.ToPasswordHash(Convert.FromBase64String(login.Salt));

            var isValid = Convert.ToBase64String(enteredPassHash) == login.PasswordHash;

            var account = await GetAccount(login.AccountId);

            return isValid ? account : null;

        }

    }
}
