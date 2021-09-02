using Microsoft.AspNetCore.Http;
using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests
{
    [TestFixture]
    class ListenerControllerTests
    {
        private Guid _id = Guid.NewGuid();

        [Test]
        public async Task GetAllListenerSongs_Id_ListSongsDto()
        {
            //Arrange
            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, _id.ToString()) };

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();
            var mockListener = new Mock<IListeners>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            var song = new Song() { Title = "title", DurationMs = 20000 };
            var songs = new List<SongDto>() { new SongDto(song) };
            var expected = new List<SongDto>() { new SongDto(song) };
            mockListener.Setup(c => c.GetAllListenerSongs(It.IsAny<Guid>())).ReturnsAsync(songs);

            var listenerController = new ListenerController(mockListener.Object);
            listenerController.ControllerContext.HttpContext = mockContext.Object;


            //Act
            var actual = await listenerController.GetAllListenerSongs();

            //Assert
            mockListener.Verify(c => c.GetAllListenerSongs(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].Title, actual.Value[0].Title);
                Assert.AreEqual(expected[0].DurationMs, actual.Value[0].DurationMs);
            });

        }


        [Test]
        public async Task GetAllListenerAlbums_Id_ListAlbumsDto()
        {
            //Arrange
            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, _id.ToString()) };

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();
            var mockListener = new Mock<IListeners>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            var album = new Album() { Name = "title" };
            var albums = new List<AlbumDto>() { new AlbumDto(album) };
            var expected = new List<AlbumDto>() { new AlbumDto(album) };
            mockListener.Setup(c => c.GetAllListenerAlbums(It.IsAny<Guid>())).ReturnsAsync(albums);

            var listenerController = new ListenerController(mockListener.Object);
            listenerController.ControllerContext.HttpContext = mockContext.Object;


            //Act
            var actual = await listenerController.GetAllListenerAlbums();

            //Assert
            mockListener.Verify(c => c.GetAllListenerAlbums(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].Name, actual.Value[0].Name);
            });
        }
    }
}
