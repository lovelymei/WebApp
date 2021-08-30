using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests
{
    [TestFixture]
    class PerformerControllerTests : ControllerBase
    {
        private Guid _id = Guid.NewGuid();

        [Test]
        public async Task GetAllPerformerSongs_IdByJwt_ListSngsDto()
        {
            //Arrange
            var mockPerformers = new Mock<IPerformers>();
            var performerController = new PerformerController(mockPerformers.Object);

            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, _id.ToString())};
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            User.AddIdentity(identity);

            var song = new Song() { Title = "title", DurationMs = 20000};
            var songs = new List<SongDto>() { new SongDto(song) };

            mockPerformers.Setup(c => c.GetAllPerformerSongs(_id)).ReturnsAsync(songs);

            var expected = new List<SongDto>() {new SongDto(song)};

            //Act
            var actual = await performerController.GetAllPerformersSongs();

            //Assert
            mockPerformers.Verify(c => c.GetAllPerformerSongs(_id), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.Equals(expected[0].Title, actual.Value[0].Title);
                Assert.Equals(expected[0].DurationMs, actual.Value[0].DurationMs);
            });




        }
    }

    
}
