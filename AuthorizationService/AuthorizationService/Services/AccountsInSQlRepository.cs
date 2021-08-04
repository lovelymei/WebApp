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

        public async Task<List<AccountDto>> GetAllAccounts()
        {
            _logger.Info($"using {nameof(GetAllAccounts)}");

            var accounts = await _db.Accounts.Where(c => c.IsDeleted == false).ToListAsync();

            if (!accounts.Any())
            {
                _logger.Warn($"{nameof(accounts)}'s list is void");
                return null;
            }

            List<AccountDto> accountsDto = new List<AccountDto>();

            foreach (var account in accounts)
            {
                accountsDto.Add(new AccountDto(account));
            }

            return accountsDto;
        }


        public async Task<Account> GetAccount(Guid id)
        {
            _logger.Info($"using {nameof(GetAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.AccountId == id && c.IsDeleted == false);

            if (account == null)
            {
                _logger.Warn($"{nameof(account)} doesn't exist");
                return null;
            }

            return account;
        }

        public async Task<bool> CheckNameEquality(string name)
        {
            var existAccounts = await GetAllAccounts();
            var existAccount = existAccounts.FirstOrDefault(a => a.NickName == name);

            if (existAccount != null)
            {
                _logger.Warn($"{nameof(existAccount)} has the entered name '{name}'");
                return true;
            }

            return false;
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
            _logger.Info($"using {nameof(RegisterListenerAccount)}");
            var listenerAccontDto = await CreateAccount(accountCreateDto, Roles.listener);
            return listenerAccontDto;
        }

        public async Task<AccountDto> RegisterPerformerAccount(AccountCreateDto accountCreateDto)
        {
            _logger.Info($"using {nameof(RegisterPerformerAccount)}");
            var performerAccountDto = await CreateAccount(accountCreateDto, Roles.performer);
            return performerAccountDto;
        }

        public async Task<AccountDto> RegisterAdminAccount(AccountCreateDto accountCreateDto)
        {
            _logger.Info($"using {nameof(RegisterAdminAccount)}");
            var adminAccountDto = await CreateAccount(accountCreateDto, Roles.administratior);
            return adminAccountDto;
        }


        public async Task<bool> UpdateAccount(Guid id, AccountCreateDto accountCreateDto)
        {
            _logger.Info($"using {nameof(UpdateAccount)}");

            var account = await _db.Accounts.Include(a => a.Login)
                                           .FirstOrDefaultAsync(c => c.AccountId == id);

            if (account == null)
            {
                _logger.Warn($"{nameof(account)} doesn't exist");
                return false;
            }

            var salt = GenerateSalt();
            var enteredPassHash = accountCreateDto.Password.ToPasswordHash(salt);

            account.NickName = accountCreateDto.NickName;
            account.Login.Email = accountCreateDto.Email;
            account.Login.Salt = Convert.ToBase64String(salt);
            account.Login.PasswordHash = Convert.ToBase64String(enteredPassHash);

            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> DeleteAccount(Guid id)
        {
            _logger.Info($"using {nameof(DeleteAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.AccountId == id);

            if (account == null)
            {
                _logger.Warn($"{nameof(account)} doesn't exist");
                return false;
            }

            account.IsDeleted = true;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<List<AccountDto>> GetAllDeletedAccounts()
        {
            _logger.Info($"using {nameof(GetAllDeletedAccounts)}");

            var deletedAccounts = await _db.Accounts.Where(c => c.IsDeleted == true).ToListAsync();

            if (!deletedAccounts.Any())
            {
                _logger.Warn($"{nameof(deletedAccounts)}'s list is void");
                return null;
            }

            List<AccountDto> accountsDto = new List<AccountDto>();

            foreach (var account in deletedAccounts)
            {
                accountsDto.Add(new AccountDto(account));
            }

            return accountsDto;
        }

        public async Task<bool> RestoreAccount(Guid id)
        {
            _logger.Info($"using {nameof(RestoreAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(l => l.AccountId == id);

            if (account == null)
            {
                _logger.Warn($"{nameof(account)} doesn't exist");
                return false;
            }

            account.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public static byte[] GenerateSalt()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[16];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        public async Task<Account> Authenticate(string email, string password)
        {
            _logger.Info($"using {nameof(Authenticate)}");

            var login = await _db.Logins.FirstOrDefaultAsync(c => c.Email == email);

            if (login == null)
            {
                _logger.Warn($"{nameof(login)} doesn't exist");
                return null;
            }

            var enteredPassHash = password.ToPasswordHash(Convert.FromBase64String(login.Salt));

            var isValid = Convert.ToBase64String(enteredPassHash) == login.PasswordHash;

            var account = await GetAccount(login.AccountId);

            return isValid ? account : null;

        }

    }
}
