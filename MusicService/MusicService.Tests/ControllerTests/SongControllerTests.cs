using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests.ControllerTests
{
    [TestFixture]
    class SongControllerTests
    {
        private const string _TITLE = "title";
        private const long _DURATION = 20000;

        [Test]
        public async Task AddSong_Title_Duration_SongDto()
        {
            //Arrange
            var mockSongs = new Mock<ISongs>();
            var mockStorage = new Mock<IStorage<SongDto>>();

            var song = new Song() { Title = _TITLE, DurationMs = _DURATION};
            var expexted = new SongDto(song);
            var songController = new SongController(mockSongs.Object, mockStorage.Object);

            mockSongs.Setup(c => c.AddSong(_TITLE, _DURATION)).ReturnsAsync(song);

            //Act
            var actual = await songController.AddSong(_TITLE, _DURATION);

            //Assert
            mockSongs.Verify(c => c.AddSong(_TITLE, _DURATION), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expexted.Title, actual.Value.Title);
                Assert.AreEqual(expexted.DurationMs, actual.Value.DurationMs);
            });


        }
    }
}
