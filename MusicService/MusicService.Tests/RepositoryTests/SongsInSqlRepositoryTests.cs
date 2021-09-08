using MusicService.Models;
using MusicService.Services;
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
    class SongsInSqlRepositoryTests
    {
        [Test]
        public async Task AddSong_TitleDuration_Song()
        {
            //Arrange
            var db = TestsRepositoryService.GetClearDataBase();

            var repository = new SongsInSQLRepository(db);
            var expected = new Song { Title = "title1", DurationMs = 20000};

            //Act
            var actual = await repository.AddSong("title1", 20000);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Title, actual.Title);
                Assert.AreEqual(expected.DurationMs, actual.DurationMs);
            });
        }
    }
}
