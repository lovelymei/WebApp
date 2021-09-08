using System;
using AuthorizationService.Controllers;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private readonly Guid _id1 = Guid.NewGuid();
        private readonly Guid _id2 = Guid.NewGuid();
        private List<AccountDto> GetAccountsDtoList()
        {
            var list = new List<AccountDto>
            {
                new AccountDto(new Account() {NickName = "acc1", IsDeleted = false}),
                new AccountDto(new Account() {NickName = "acc2", IsDeleted = false})
            };

            return list;
        }

        private AccountCreateDto CreateAccountDto()
        {
            var email = "mail";
            var password = "password";
            var nickname = "testAccount";
            var accountCreateDto = new AccountCreateDto
            {
                NickName = nickname,
                Email = email,
                Password = password
            };
            return accountCreateDto;
        }

        private List<AccountDto> GetDeletedAccountsDtoList()
        {
            var list = new List<AccountDto>
            {
                new AccountDto(new Account() {NickName = "acc1", IsDeleted = true}),
                new AccountDto(new Account() {NickName = "acc2", IsDeleted = true})
            };

            return list;
        }
        
        private List<Account> GetAccountsList()
        {
            var list = new List<Account>
            {
                new Account() {NickName = "acc1", IsDeleted = false},
                new Account() {NickName = "acc2", IsDeleted = false},
                new Account() {NickName = "acc3", IsDeleted = false}
            };

            return list;
        }

        private Account GetSingleAccount()
        {
            var expectedLogin = new Login()
            {
                Email = "mail"
            };
            var account = new Account()
            {
                IsDeleted = false,
                EntityId = _id1,
                NickName = "testAccount",
                Login = expectedLogin
            };

            return account;
        }

        [Test]
        public async Task GetAllAccounts_AccountsReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var expected = GetAccountsDtoList();
            mockAccounts.Setup(c => c.GetAllAccountsDto()).ReturnsAsync(GetAccountsDtoList());
            var accountController = new AccountController(mockAccounts.Object);

            //Act
            var actual = await accountController.GetAllAccounts();

            //Assert
            mockAccounts.Verify(c => c.GetAllAccountsDto(), Times.Once);

            Assert.AreEqual(expected.Count, actual.Count);

            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual[i].NickName);
                    Assert.AreEqual(expected[i].Role, actual[i].Role);
                }
            });
        }

        [Test]
        public async Task GetAllDeletedAccounts_DeletedAccountsReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var expected = GetDeletedAccountsDtoList();
            mockAccounts.Setup(c => c.GetAllDeletedAccounts()).ReturnsAsync(GetDeletedAccountsDtoList());

            //Act
            var actual = await accountController.GetAllDeletedAccounts();

            //assert
            mockAccounts.Verify(c => c.GetAllDeletedAccounts(), Times.Once);
            Assert.AreEqual(expected.Count, actual.Value.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].NickName, actual.Value[i].NickName);
                    Assert.AreEqual(expected[i].Role, actual.Value[i].Role);
                }
            });
        }

        //добавить второй тест, где аккаунт не найден
        [Test]
        public async Task GetCurrentAccount_ExistingId_AccountReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            

            var expectedDto = new AccountDto(GetSingleAccount());
            
            mockAccounts.Setup(c => c.GetCurrentAccount(_id1)).ReturnsAsync(GetSingleAccount());
            
            //act
            var actual = await accountController.GetCurrentAccount(_id1);
            
            //Assert

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedDto.Role,actual.Value.Role);
                Assert.AreEqual(expectedDto.NickName,actual.Value.NickName);
                
            });
        }

        [Test]
        public async Task GetCurrentAccount_NotExistingId_AccountNotReceived()
        {
            //Arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            
            var expectedDto = new AccountDto(GetSingleAccount());
            
            mockAccounts.Setup(c => c.GetCurrentAccount(_id1)).ReturnsAsync(GetSingleAccount());
            
            //act
            var actual = await accountController.GetCurrentAccount(_id2);
            
            //Assert
            Assert.IsNull(actual);
        }
        
        [Test]
        public async Task DeleteAccount_Id_AccountDeleted()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var expected = new OkResult();
            
            mockAccounts.Setup(c => c.DeleteAccount(_id1)).ReturnsAsync(true);

            //act
            var actual = await accountController.DeleteAccount(_id1) as OkResult;
            
            //assert
            Assert.AreEqual(expected.StatusCode,actual.StatusCode);
        }

        [Test]
        public async Task DeleteAccount_NotExistingId_AccountNotDeleted()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var expected = new NotFoundResult();
            
            mockAccounts.Setup(c => c.DeleteAccount(_id2)).ReturnsAsync(true);

            //act
            var actual = await accountController.DeleteAccount(_id1) as NotFoundResult;
            
            //assert
            Assert.AreEqual(expected.StatusCode,actual.StatusCode);
        }
        
        [Test]
        public async Task RegisterListenerAccount_AccountInfo_ListenerCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var email = "email";
            var password = "password";
            var nickname = "single";
            
            var accountCreateDto = new AccountCreateDto
            {
                NickName = nickname,
                Email = email,
                Password = password
            };
            
            var role = Roles.listener;

            mockAccounts.Setup(a => a.CheckNameEquality(nickname)).ReturnsAsync(false);
            mockAccounts
                .Setup(a => a.CreateAccount(accountCreateDto, role))
                .ReturnsAsync(new AccountDto(GetSingleAccount()));
            var expectedResult = new OkResult();
            
            var actual = await accountController.RegisterListenerAccount(accountCreateDto) as OkResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }
        
        [Test]
        public async Task RegisterListenerAccount_ExistingAccountInfo_ListenerNotCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            
            var role = Roles.listener;
            var accountCreateDto = CreateAccountDto();

            var expected = new AccountDto(GetSingleAccount());
            var expectedResult = new ConflictResult();
            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(true);

            var actual = await accountController
                .RegisterListenerAccount(accountCreateDto) as ConflictResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }

        [Test]
        public async Task RegisterPerformerAccount_AccountInfo_PerformerCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();
            
            var role = Roles.performer;

            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(false);
            
            mockAccounts
                .Setup(a => a.CreateAccount(accountCreateDto, role))
                .ReturnsAsync(new AccountDto(GetSingleAccount()));
            
            var expectedResult = new OkResult();
            
            var actual = await accountController.RegisterPerformerAccount(accountCreateDto) as OkResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }

        [Test]
        public async Task RegisterPerformerAccount_ExistingAccountInfo_PerformerNotCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();
            
            var role = Roles.performer;

            var expected = new AccountDto(GetSingleAccount());
            var expectedResult = new ConflictResult();
            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(true);

            var actual = await accountController
                .RegisterPerformerAccount(accountCreateDto) as ConflictResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }
        [Test]
        public async Task RegisterAdminAccount_AccountInfo_AdminCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();
            
            var role = Roles.administratior;

            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(false);
            
            mockAccounts
                .Setup(a => a.CreateAccount(accountCreateDto, role))
                .ReturnsAsync(new AccountDto(GetSingleAccount()));
            
            var expectedResult = new OkResult();
            
            var actual = await accountController.RegisterAdminAccount(accountCreateDto) as OkResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }

        [Test]
        public async Task RegisterAdminAccount_ExistingAccountInfo_AdminNotCreated()
        {
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();
            
            var role = Roles.administratior;

            var expected = new AccountDto(GetSingleAccount());
            var expectedResult = new ConflictResult();
            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(true);

            var actual = await accountController
                .RegisterAdminAccount(accountCreateDto) as ConflictResult;
            
            //assert
            Assert.AreEqual(expectedResult.StatusCode, actual.StatusCode);
        }

        [Test] //Ok
        public async Task UpdateAccount_AccountInfo_AccountUpdated()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();

            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(false);
            mockAccounts.Setup(a => a.UpdateAccount(_id1, accountCreateDto)).ReturnsAsync(true);
            var expectedResult = new OkResult();
            //act
            var actual = await accountController.UpdateAccount(_id1,accountCreateDto) as OkResult;

            //assert
            Assert.AreEqual(expectedResult.StatusCode,actual.StatusCode);
        }
        
        [Test] //Conflict
        public async Task UpdateAccount_ExistingAccountInfo_AccountNotUpdated()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();

            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(true);
            var expectedResult = new ConflictResult();
            //act
            var actual = await accountController.UpdateAccount(_id1,accountCreateDto) as ConflictResult;

            //assert
            Assert.AreEqual(expectedResult.StatusCode,actual.StatusCode);
        }
        
        [Test] //NotFound
        public async Task UpdateAccount_AccountInfo_AccountNotFound()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            var accountCreateDto = CreateAccountDto();

            mockAccounts.Setup(a => a.CheckNameEquality(accountCreateDto.NickName)).ReturnsAsync(false);
            mockAccounts.Setup(a => a.UpdateAccount(_id1, accountCreateDto)).ReturnsAsync(false);
            var expectedResult = new NotFoundResult();
            //act
            var actual = await accountController.UpdateAccount(_id1,accountCreateDto) as NotFoundResult;

            //assert
            Assert.AreEqual(expectedResult.StatusCode,actual.StatusCode);
        }

        [Test]
        public async Task RestoreAccount_Id_AccountRestored()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            mockAccounts.Setup(a => a.RestoreAccount(_id1)).ReturnsAsync(true); //change this

            var expected = new OkResult();
            //act
            var actual = await accountController.RestoreAccount(_id1) as OkResult;
            
            //assert
            Assert.AreEqual(expected.StatusCode,actual.StatusCode);

        }
        
        [Test]
        public async Task RestoreAccount_NotExistingId_AccountNotRestored()
        {
            //arrange
            var mockAccounts = new Mock<IAccounts>();
            var accountController = new AccountController(mockAccounts.Object);
            mockAccounts.Setup(a => a.RestoreAccount(_id1)).ReturnsAsync(false); //change this

            var expected = new NotFoundResult();
            //act
            var actual = await accountController.RestoreAccount(_id1) as NotFoundResult;
            
            //assert
            Assert.AreEqual(expected.StatusCode,actual.StatusCode);

        }
    }
}