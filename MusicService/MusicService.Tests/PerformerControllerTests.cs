using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MusicService.Tests
{
    public class PerformerControllerTests : ControllerBase
    {
        private Guid _songId = Guid.NewGuid();
        private Guid _albumId = Guid.NewGuid();

        [Fact]
        public async Task GetAllPerformerSongs_Id_ListSngsDto()
        {
            //Arrange
            var mockPerformers = new Mock<IPerformers>();

            var song = new Song() { Title = "title", DurationMs = 20000 };
            var songs = new List<SongDto>() { new SongDto(song) };
            var expected = new List<SongDto>() { new SongDto(song) };
            mockPerformers.Setup(c => c.GetAllPerformerSongs(It.IsAny<Guid>())).ReturnsAsync(songs);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = TestsService.SetHttpContext(); 
            

            //Act
            var actual = await performerController.GetAllPerformerSongs();

            //Assert
            mockPerformers.Verify(c => c.GetAllPerformerSongs(It.IsAny<Guid>()), Times.Once);

            Assert.Equal(expected[0].Title, actual.Value[0].Title);
            Assert.Equal(expected[0].DurationMs, actual.Value[0].DurationMs);
    
        }


        [Fact]
        public async Task GetAllPerformersAlbums_Id_ListAlbumsDto()
        {
            //Arrange
            var mockPerformers = new Mock<IPerformers>();

            var album = new Album() { Name = "title"};
            var albums = new List<AlbumDto>() { new AlbumDto(album) };
            var expected = new List<AlbumDto>() { new AlbumDto(album) };
            mockPerformers.Setup(c => c.GetAllPerformerAlbums(It.IsAny<Guid>())).ReturnsAsync(albums);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = TestsService.SetHttpContext();


            //Act
            var actual = await performerController.GetAllPerformerAlbums();

            //Assert
            mockPerformers.Verify(c => c.GetAllPerformerAlbums(It.IsAny<Guid>()), Times.Once);

            Assert.Equal(expected[0].Name, actual.Value[0].Name); 
       }


        [Fact]
        public async Task AttachSongToPerformer_SongId_ResponceCode()
        {
            //Arrange
            var mockPerformers = new Mock<IPerformers>();

            var song = new Song() { Title = "title", DurationMs = 20000 };
            var expected = new OkResult();
            mockPerformers.Setup(c => c.AttachSong(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await performerController.AttachSongToPerformer(_songId) as OkResult;

            //Assert
            mockPerformers.Verify(c => c.AttachSong(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);


            Assert.Equal(expected.StatusCode, actual.StatusCode);



        }

        [Fact]
        public async Task AttachAlbumToPerformer_AlbumId_ResponceCode()
        {
            //Arrange
            var mockPerformers = new Mock<IPerformers>();

            var album = new Album() { Name = "title"};
            var expected = new OkResult();
            mockPerformers.Setup(c => c.AttachAlbum(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var performerController = new PerformerController(mockPerformers.Object);
            performerController.ControllerContext.HttpContext = TestsService.SetHttpContext();

            //Act
            var actual = await performerController.AttachAlbumToPerformer(_albumId) as OkResult;

            //Assert
            mockPerformers.Verify(c => c.AttachAlbum(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);


            Assert.Equal(expected.StatusCode, actual.StatusCode);

        }

    }

    
}
