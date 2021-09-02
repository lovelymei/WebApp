using Microsoft.AspNetCore.Http;
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
        public async Task GetAllPerformerSongs_Id_ListSngsDto()
        {
            //Arrange
            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, _id.ToString()) };

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();
            var mockPerformers = new Mock<IPerformers>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            var song = new Song() { Title = "title", DurationMs = 20000 };
            var songs = new List<SongDto>() { new SongDto(song) };
            var expected = new List<SongDto>() { new SongDto(song) };
            mockPerformers.Setup(c => c.GetAllPerformerSongs(It.IsAny<Guid>())).ReturnsAsync(songs);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = mockContext.Object; 
            

            //Act
            var actual = await performerController.GetAllPerformerSongs();

            //Assert
            mockPerformers.Verify(c => c.GetAllPerformerSongs(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].Title, actual.Value[0].Title);
                Assert.AreEqual(expected[0].DurationMs, actual.Value[0].DurationMs);
            });

        }


        [Test]
        public async Task GetAllPerformersAlbums_Id_ListAlbumsDto()
        {
            //Arrange
            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, _id.ToString()) };

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();
            var mockPerformers = new Mock<IPerformers>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            var album = new Album() { Name = "title"};
            var albums = new List<AlbumDto>() { new AlbumDto(album) };
            var expected = new List<AlbumDto>() { new AlbumDto(album) };
            mockPerformers.Setup(c => c.GetAllPerformerAlbums(It.IsAny<Guid>())).ReturnsAsync(albums);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = mockContext.Object;


            //Act
            var actual = await performerController.GetAllPerformerAlbums();
            
            //Assert
            mockPerformers.Verify(c => c.GetAllPerformerAlbums(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].Name, actual.Value[0].Name);
            });
        }



    }

    
}
