using Microsoft.Extensions.Logging;
using Moq;
using MusicService.Entity;
using MusicService.Models;
using MusicService.Services;
using MusicService.Tests.Extensions;
using MusicService.Tests.TestsServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests.RepositoryTests
{
    [TestFixture]
    class AlbumsInSqlRepositoryTests
    {
        [Test]
        public async Task AddAlbum_AlbumTitle_Album()
        { 
            //Arrange
            var db = TestsRepositoryService.GetClearDataBase();

            var repository = new AlbumsInSQLRepository(db);
            var expected = new Album { Name = "title1" };

            //Act
            var actual = await repository.AddAlbum("title1");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Name, actual.Name);
            });
        }
    }
}
