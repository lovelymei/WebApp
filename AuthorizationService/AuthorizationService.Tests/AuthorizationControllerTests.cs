using System;
using System.Threading.Tasks;
using AuthorizationService.Controllers;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class AuthorizationControllerTests
    {
        private readonly Guid _id0 = Guid.NewGuid();
        private readonly Guid _id1 = Guid.NewGuid();
        
        private Account GetSingleAccount()
        {
            var login = new Login()
            {
                Email = "mail",
                Salt = "bjdfhuheye9u83t",
                AccountId = _id1,
                PasswordHash = "hasut59uh78rbnr9h"

            };
            var account = new Account()
            {
                NickName = "name",
                IsDeleted = false,
                Login = login,
                EntityId = _id1,
                Role = Roles.performer
            };
            return account;
        }

        private RefreshToken GetSignleRefreshToken()
        {
            var token = new RefreshToken()
            {
                AccountId = _id1,
                CreateDate = DateTime.Now,
                ExpiresDate = DateTime.Today,
                RefreshTokenId = _id0
            };
            return token;
        }
        

        [Test]
        public async Task CreateToken_SignInInfo_TokenCreated()
        {
            //arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockAccounts = new Mock<IAccounts>();
            var mockRefreshTokens = new Mock<IRefreshTokens>();
            var authorizationController = new AuthorizationController(mockAccounts.Object,
                mockConfiguration.Object,
                mockRefreshTokens.Object);
            var signIn = new SignIn()
            {
                Email = "mail",
                Password = "123"
            };
            var refreshToken = GetSignleRefreshToken();
            var account = GetSingleAccount();
            var expected = new OkResult();
            
            mockAccounts.Setup(a => a.Authenticate(signIn.Email, signIn.Password))
                .ReturnsAsync(GetSingleAccount());

            mockRefreshTokens.Setup(a => a.CreateRefreshToken(account, 86400))
                .ReturnsAsync(GetSignleRefreshToken());

            //act
            var actual = await authorizationController.CreateToken(signIn) as OkResult;
            
            //assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [Test]
        public async Task RefreshToken_Id_RefreshTokenCreated()
        {
            
        }
        
    }
}