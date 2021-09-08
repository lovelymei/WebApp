using Microsoft.AspNetCore.Http;
using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MusicService.Tests
{
    public class ListenerControllerTests
    {
        private Guid _songId = Guid.NewGuid();
        private Guid _albumId = Guid.NewGuid();

        [Fact]
        public async Task GetAllListenerSongs_Id_ListSongsDto()
        {
            //Arrange;
            var mockListener = new Mock<IListeners>();
            var song = new Song() {Title = "title", DurationMs = 20000};
            var songs = new List<SongDto>() {new SongDto(song)};
            var expected = new List<SongDto>() {new SongDto(song)};
            mockListener.Setup(c => c.GetAllListenerSongs(It.IsAny<Guid>())).ReturnsAsync(songs);
            var listenerController = new ListenerController(mockListener.Object);
            listenerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await listenerController.GetAllListenerSongs();

            //Assert
            mockListener.Verify(c => c.GetAllListenerSongs(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(expected[0].Title, actual.Value[0].Title);
            Assert.Equal(expected[0].DurationMs, actual.Value[0].DurationMs);
        }


        [Fact]
        public async Task GetAllListenerAlbums_Id_ListAlbumsDto()
        {
            //Arrange
            var mockListener = new Mock<IListeners>();
            var album = new Album() {Name = "title"};
            var albums = new List<AlbumDto>() {new AlbumDto(album)};
            var expected = new List<AlbumDto>() {new AlbumDto(album)};
            mockListener.Setup(c => c.GetAllListenerAlbums(It.IsAny<Guid>())).ReturnsAsync(albums);
            var listenerController = new ListenerController(mockListener.Object);
            listenerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await listenerController.GetAllListenerAlbums();

            //Assert
            mockListener.Verify(c => c.GetAllListenerAlbums(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(expected[0].Name, actual.Value[0].Name);
        }

        [Fact]
        public async Task AttachSongToListener_SongId_ResponceCode()
        {
            //Arrange
            var mockPerformers = new Mock<IListeners>();
            var song = new Song() {Title = "title", DurationMs = 20000};
            var expected = new OkResult();
            mockPerformers.Setup(c => c.AttachSong(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var listenerController = new ListenerController(mockPerformers.Object);
            listenerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await listenerController.AttachSongToListener(_songId) as OkResult;

            //Assert
            mockPerformers.Verify(c => c.AttachSong(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.Equal(expected.StatusCode, actual.StatusCode);
        }

        [Fact]
        public async Task AttachAlbumToListener_AlbumId_ResponceCode()
        {
            //Arrange
            var mockListeners = new Mock<IListeners>();

            var album = new Album() {Name = "title"};
            var expected = new OkResult();
            mockListeners.Setup(c => c.AttachAlbum(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var listenerController = new ListenerController(mockListeners.Object);
            listenerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await listenerController.AttachAlbumToListener(_albumId) as OkResult;

            //Assert
            mockListeners.Verify(c => c.AttachAlbum(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);


            Assert.Equal(expected.StatusCode, actual.StatusCode);
        }
    }
}