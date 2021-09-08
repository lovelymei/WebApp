using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorizationService.Dto;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Tests
{
    [TestFixture]
    public class RefreshTokensInSQlRepositoryTests
    {
        private readonly Guid _id0 = Guid.NewGuid();
        private readonly Guid _id1 = Guid.NewGuid();
        private readonly Guid _id2 = Guid.NewGuid();
        private readonly Guid _id3 = Guid.NewGuid();
        private readonly Guid _id4 = Guid.NewGuid();
        private readonly Guid _id5 = Guid.NewGuid();

        private readonly DateTime createDate = DateTime.Now;
        private readonly DateTime expiresDate = DateTime.UtcNow;
        
        
        private AuthorizationDbContext GetClearDataBase()
        {
            var options = new DbContextOptionsBuilder<AuthorizationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AuthorizationDbContext(options);
        }

        private List<RefreshToken> GetRefreshTokenList()
        {
            var list = new List<RefreshToken>();
            var token = new RefreshToken()
            {
                AccountId = _id0,
                CreateDate = createDate,
                ExpiresDate = expiresDate,
                RefreshTokenId = _id1
            };
            list.Add(token);
            token = new RefreshToken()
            {
                AccountId = _id0,
                CreateDate = createDate,
                ExpiresDate = expiresDate,
                RefreshTokenId = _id2
            };
            list.Add(token);
            token = new RefreshToken()
            {
                AccountId = _id0,
                CreateDate = createDate,
                ExpiresDate = expiresDate,
                RefreshTokenId = _id3
            };
            list.Add(token);
            token = new RefreshToken()
            {
                AccountId = _id0,
                CreateDate = createDate,
                ExpiresDate = expiresDate,
                RefreshTokenId = _id4
            };
            list.Add(token);
            token = new RefreshToken()
            {
                AccountId = _id0,
                CreateDate = createDate,
                ExpiresDate = expiresDate,
                RefreshTokenId = _id5
            };
            list.Add(token);
            return list;
        }

        private void FillDatabaseWithData(AuthorizationDbContext db)
        {
            var list = GetRefreshTokenList();
            foreach (var token in list)
            {
                db.Add(token);
            }

            db.SaveChanges();
        }
        

        [Test]
        public async Task GetAllRefreshTokens_TokensReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<RefreshTokensInSqlRepository>>();
            var repository = new RefreshTokensInSqlRepository(database,mockLogger.Object);
            var expected = GetRefreshTokenList();
            
            //act
            var actualIEnumerable = await repository.GetAllRefreshTokens();
            List<RefreshTokenDto> actual = new List<RefreshTokenDto>();
            foreach (var refreshTokenDto in actualIEnumerable)
            {
                actual.Add(refreshTokenDto);
            }
            
            Assert.AreEqual(expected.Count,actual.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].AccountId,actual[i].AccountId);
                    Assert.AreEqual(expected[i].CreateDate,actual[i].CreateDate);
                    Assert.AreEqual(expected[i].ExpiresDate,actual[i].ExpiresDate);
                    Assert.AreEqual(expected[i].RefreshTokenId,actual[i].RefreshTokenId);
                }
            });

        }

        [Test]
        public async Task GetAllAccountRefreshTokens_ExistingId_TokensReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<RefreshTokensInSqlRepository>>();
            var repository = new RefreshTokensInSqlRepository(database,mockLogger.Object);
            var expected = GetRefreshTokenList();//использую эту функцию, потому что там у всех токенов один AccountId - id0

            var actualIEnumerable = await repository.GetAllAccountRefreshTokens(_id0);
            List<RefreshTokenDto> actual = new List<RefreshTokenDto>();
            foreach (var refreshTokenDto in actualIEnumerable)
            {
                actual.Add(refreshTokenDto);
            }
            
            Assert.AreEqual(expected.Count,actual.Count);
            Assert.Multiple(() =>
            {
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].AccountId,actual[i].AccountId);
                    Assert.AreEqual(expected[i].CreateDate,actual[i].CreateDate);
                    Assert.AreEqual(expected[i].ExpiresDate,actual[i].ExpiresDate);
                    Assert.AreEqual(expected[i].RefreshTokenId,actual[i].RefreshTokenId);
                }
            });

        }

        [Test]
        public async Task GetAllAccountRefreshTokens_NotExistingId_TokensNotReceived()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<RefreshTokensInSqlRepository>>();
            var repository = new RefreshTokensInSqlRepository(database, mockLogger.Object);
            var expected =
                GetRefreshTokenList(); //использую эту функцию, потому что там у всех токенов один AccountId - id0

            var actualIEnumerable =
                await repository.GetAllAccountRefreshTokens(_id2); //такой id стоит у одного из токенов

            Assert.IsEmpty(actualIEnumerable);
        }

        [Test]
        public async Task DeleteRefreshTokenForAccount_ExistingId_ReturnTrue()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<RefreshTokensInSqlRepository>>();
            var repository = new RefreshTokensInSqlRepository(database, mockLogger.Object);

            var actual = await repository.DeleteRefreshTokensForAccount(_id0);
            
            Assert.IsTrue(actual);
        }
        
        [Test]
        public async Task DeleteRefreshTokenForAccount_NotExistingId_ReturnFalse()
        {
            var database = GetClearDataBase();
            FillDatabaseWithData(database);
            var mockLogger = new Mock<ILogger<RefreshTokensInSqlRepository>>();
            var repository = new RefreshTokensInSqlRepository(database, mockLogger.Object);

            var actual = await repository.DeleteRefreshTokensForAccount(_id1);
            
            Assert.IsFalse(actual);
        }
        
    }
}