using System;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class AccountsInSQlRepositoryTests
    {
        private readonly Guid _id0 = Guid.NewGuid();
        private readonly Guid _id1 = Guid.NewGuid();
        private readonly Guid _id2 = Guid.NewGuid();
        private readonly Guid _id3 = Guid.NewGuid();
        private readonly Guid _id4 = Guid.NewGuid();


        private AuthorizationDbContext GetClearDataBase()
        {
            var options = new DbContextOptionsBuilder<AuthorizationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AuthorizationDbContext(options);
        }


        private Account GetSingleAccount()
        {
            var login = new Login
            {
                Email = "mail",
                Salt = "dkjivjdfoijigju958978=",
                PasswordHash = "passwordHASHSHSHSH",
                AccountId = _id0
            };
            return new Account()
            {
                NickName = "single", 
                EntityId = _id0,
                Login = login
            };

        }
        private List<Account> GetAccountsList()
        {
            var list = new List<Account>();
            var login = new Login
            {
                Email = "mail",
                Salt = "dkjivjdfoijigju958978=",
                PasswordHash = "passwordHASHSHSHSH"
            };
            var account = new Account {NickName = "acc1", EntityId = _id1,Login = login};
            list.Add(account);
            account = new Account {NickName = "acc2", EntityId = _id2,Login = login};
            list.Add(account);
            account = new Account {NickName = "acc3", EntityId = _id3,Login = login};
            list.Add(account);
            account = new Account {NickName = "acc4", EntityId = _id4,Login = login};
            list.Add(account);
            return list;
        }

        private List<Account> GetDeletedAccountsList()
        {
            var list = GetAccountsList();
            list.ForEach(c => c.IsDeleted = true);
            return list;
        }

        private void FillDatabaseWithData(AuthorizationDbContext db)
        {
            var list = GetAccountsList();
            foreach (var account in list)
            {
                db.Add(account);
            }

            db.SaveChanges();
        }

        private void FillDatabaseWithDeletedData(AuthorizationDbContext db)
        {
            var list = GetDeletedAccountsList();
            foreach (var account in list)
            {
                db.Add(account);
            }

            db.SaveChanges();
        }


        [Test]
        public async Task GetAllAccounts_AccountsReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var expected = GetAccountsList();

            //act
            var actualIEnumerable = await repository.GetAllAccountsDto();

            //assert
            List<AccountDto> actual = new List<AccountDto>();

            foreach (var account in actualIEnumerable)
            {
                actual.Add(account);
            }

            Assert.AreEqual(expected.Count, actual.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual[i].NickName);
                    Assert.AreEqual(expected[i].Role.ToString(), actual[i].Role);
                }
            });
        }


        [Test]
        public async Task GetAccount_AccountId_AccountReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);

            var expected = GetAccountsList();
            var expectedInstance = expected[1];


            var actual = await repository.GetCurrentAccount(_id2);


            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedInstance.NickName, actual.NickName);
                Assert.AreEqual(expectedInstance.Role, actual.Role);
                Assert.AreEqual(expectedInstance.EntityId, actual.EntityId);
                Assert.AreEqual(expectedInstance.IsDeleted, actual.IsDeleted);
            });
        }

        [Test]
        public async Task CheckNameEquality_ExistingNickname_ReturnTrue()
        {
            //arrange
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var existingNickname = "acc1";
            //act
            var actual = await repository.CheckNameEquality(existingNickname);

            //assert
            Assert.IsTrue(actual);
        }

        [Test]
        public async Task CheckNameEquality_NotExistingNickname_ReturnFalse()
        {
            //arrange
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var notExistingNickname = "notExistingNickname";
            //act
            var actual = await repository.CheckNameEquality(notExistingNickname);

            //assert
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task CreateAccount_AccountInfo_NewAccountDto()
        {
            //arrange
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var nickname = "nickname";
            var email = "email";
            var password = "passWORD";

            var accountCreateDto = new AccountCreateDto
            {
                NickName = nickname,
                Email = email,
                Password = password
            };
            var role = Roles.listener;
            var expectedLoginViewModel = new Login
            {
                Email = email
            };
            //QUESTION: что делать с солью?
            //QUESTION: почему в тесте вылетает ошибка на Dispose или зачем в тестируемом методе это использовалось
            var expectedDbAccount = new Account
            {
                Login = expectedLoginViewModel,
                Role = role,
                NickName = nickname
            };
            var expectedAccountDto = new AccountDto(expectedDbAccount);
            //act
            var actualAccountDto = await repository.CreateAccount(accountCreateDto, role);

            //assert
            var actualDbAccount = await database.Accounts
                .FirstOrDefaultAsync(a => a.NickName == nickname);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedAccountDto.Role, actualAccountDto.Role);
                Assert.AreEqual(expectedAccountDto.NickName, actualAccountDto.NickName);

                var expectedLogin = expectedDbAccount.Login;
                var actualLogin = actualDbAccount.Login;
                Assert.AreEqual(expectedLogin.Email, actualLogin.Email);
                Assert.NotNull(actualLogin.Salt);
                Assert.NotNull(actualLogin.PasswordHash);

                Assert.AreEqual(expectedDbAccount.Role, actualDbAccount.Role);
                Assert.AreEqual(expectedDbAccount.NickName, actualDbAccount.NickName);
            });
        }

        [Test]
        public async Task UpdateAccount_ExistingAccountInfo_ReturnTrue()
        {
            //arrange
            var database = GetClearDataBase();
            database.Add(GetSingleAccount());
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var existingId = _id0;

            var accountCreateDto = new AccountCreateDto
            {
                Email = "newEmail",
                Password = "newPassword",
                NickName = "newNickname"
            };

            //act
            var actual = await repository.UpdateAccount(existingId, accountCreateDto);

            //assert
            Assert.IsTrue(actual);
        }

        [Test]
        public async Task UpdateAccount_ExistingAccountInfo_ReturnFalse()
        {
            var database = GetClearDataBase();
            database.Add(GetSingleAccount());
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var notExistingId = _id2;

            var accountCreateDto = new AccountCreateDto
            {
                Email = "newEmail",
                Password = "newPassword",
                NickName = "newNickname"
            };

            //act
            var actual = await repository.UpdateAccount(notExistingId, accountCreateDto);

            //assert
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task DeleteAccount_ExistingAccountId_ReturnTrue()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var existingId = _id1;

            var actual = await repository.DeleteAccount(existingId);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task DeleteAccount_NotExistingAccountId_ReturnFalse()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var existingId = _id0;

            var actual = await repository.DeleteAccount(existingId);

            Assert.IsFalse(actual);
        }


        [Test]
        public async Task GetAllDeletedAccounts_AccountReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithDeletedData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var expected = GetDeletedAccountsList();

            //act
            var actualIEnumerable = await repository.GetAllDeletedAccounts();

            //assert
            List<AccountDto> actual = new List<AccountDto>();

            foreach (var account in actualIEnumerable)
            {
                actual.Add(account);
            }

            Assert.AreEqual(expected.Count, actual.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual[i].NickName);
                    Assert.AreEqual(expected[i].Role.ToString(), actual[i].Role);
                }
            });
        }

        [Test]
        public async Task RestoreAccount_ExistingId_ReturnTrue()
        {
            var database = GetClearDataBase();
            FillDatabaseWithDeletedData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var existingId = _id1;

            var actual = await repository.RestoreAccount(existingId);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task RestoreAccount_NotExistingId_ReturnFalse()
        {
            var database = GetClearDataBase();
            FillDatabaseWithDeletedData(database);
            var mockLogger = new Mock<ILogger<AccountsInSQlRepository>>();
            var repository = new AccountsInSQlRepository(database, mockLogger.Object);
            var notExistingId = _id0;

            var actual = await repository.RestoreAccount(notExistingId);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task Authenticate_LoginInfo_ReturnAuthenticated_Account()
        {
            //не уверена, что здесь можно что-либо протестировать
        }
    }
}