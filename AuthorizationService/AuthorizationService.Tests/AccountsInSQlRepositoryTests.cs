using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class AccountsInSQlRepositoryTests
    {
        private readonly Guid _serviceStatusId = Guid.NewGuid();
        private readonly Guid _id1 = Guid.NewGuid();
        private readonly Guid _id2 = Guid.NewGuid();
        private readonly Guid _id3 = Guid.NewGuid();
        private readonly Guid _id4 = Guid.NewGuid();
        private readonly Guid _id5 = Guid.NewGuid();
        
        private AuthorizationDbContext GetClearDataBase()
        {
            var options = new DbContextOptionsBuilder<AuthorizationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            return new AuthorizationDbContext(options);
        }

        private List<Account> GetAccountsList()
        {
            var list = new List<Account>();
            var account = new Account {NickName = "acc1", EntityId = _id5};
            list.Add(account);
            account = new Account {NickName = "acc2", EntityId = _id2};
            list.Add(account);
            account = new Account {NickName = "acc3",EntityId = _id3};
            list.Add(account);
            account = new Account {NickName = "acc4",EntityId = _id4};
            list.Add(account);
            return list;
        }
        
        private void FillDatabaseWithData(AuthorizationDbContext db)
        {
            var list = GetAccountsList();
            foreach(var account in list)
            {
                db.Add(account);
            }
            db.SaveChanges();
        }
        
        [Test]
        public void CreateSuperAdmin_AdminCreated()
        {
            var mockAuthorization = new Mock<AuthorizationDbContext>();
            

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
            var actualIEnumerable = await repository.GetAllAccounts();
            

            //assert
            List<AccountDto> actual = new List<AccountDto>();
            
            foreach(var account in actualIEnumerable)
            {
                actual.Add(account);
            }
            
            Assert.AreEqual(expected.Count,actual.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName,actual[i].NickName);
                    Assert.AreEqual(expected[i].Role.ToString(),actual[i].Role);
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
           

            var actual = await repository.GetAccount(_id2);
            
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedInstance.NickName,actual.NickName);
                Assert.AreEqual(expectedInstance.Role,actual.Role);
                Assert.AreEqual(expectedInstance.EntityId,actual.EntityId);
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
            var actualAccountDto = await repository.CreateAccount(accountCreateDto,role);
            
            //assert
            var actualDbAccount = await database.Accounts
                .FirstOrDefaultAsync(a => a.NickName == nickname);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedAccountDto.Role,actualAccountDto.Role);
                Assert.AreEqual(expectedAccountDto.NickName,actualAccountDto.NickName);
            
                var expectedLogin = expectedDbAccount.Login;
                var actualLogin = actualDbAccount.Login;
                Assert.AreEqual(expectedLogin.Email,actualLogin.Email);
                Assert.NotNull(actualLogin.Salt);
                Assert.NotNull(actualLogin.PasswordHash);
            
                Assert.AreEqual(expectedDbAccount.Role,actualDbAccount.Role);
                Assert.AreEqual(expectedDbAccount.NickName, actualDbAccount.NickName);
            });
            

        }

        [Test]
        public async Task UpdateAccount_ExistingAccountInfo_ReturnTrue()
        {
            
        }
       
        
    }
    
    
}



























