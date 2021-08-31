using Moq;
using MusicService.Controllers;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MusicService.Tests
{
    [TestFixture] //класс для тестов
    public class AlbumControllerTests
    {
        private const string albumTitle = "title";

        [Test] 
        //название тестируемой функции_передаваемый объект_ожидаемый результат
        public async Task AddAlbum_AlbumTitle_AlbumAdded()
        {
            //Arrange <-> Setup
            //подготовка объектов(данных) к тесту
            var mockAlbums = new Mock<IAlbums>();
            var mockStorage = new Mock<IStorage<AlbumDto>>();
            var album = new Album() { Name = albumTitle};
            var expected = new AlbumDto(album); //ожидаемый объект
            var albumController = new AlbumController(mockAlbums.Object,mockStorage.Object);
            mockAlbums.Setup(c => c.AddAlbum(albumTitle)).ReturnsAsync(album);

            //Act
            //объект, который вернулся на самом деле
            var actual = await albumController.AddAlbum(albumTitle); //returns AlbumDto

            //Assert
            //проверить, совпадает ли ожидание с реальностью
            //actual и expected - разные объекты, их не сравнить напрямую, надо сравнивать их поля

            //проверка, что AddAlbum вызвалась один раз
            mockAlbums.Verify(c => c.AddAlbum(albumTitle),Times.Once);

            Assert.Multiple(() => //форма для множества проверок 
            {
                Assert.AreEqual(expected.Name, actual.Value.Name);
                Assert.AreEqual(expected.ProductionDate, actual.Value.ProductionDate);
                Assert.AreEqual(expected.IsDeleted, actual.Value.IsDeleted);
            });    
        }
    }
}
