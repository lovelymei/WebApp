using Moq;
using MusicService.Controllers;
using MusicService.Entity;
using MusicService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests
{
    [TestFixture]
    class CrudController
    {
        public object Time { get; private set; }

        [Test]
        public async Task GetAll_Void_EntitiesList()
        {
           
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();
            
            var expected = new List<EntityBaseDto>()
            { 
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()})
            };

            var entityBaseList = expected;

            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.GetAllEntitiesDto()).ReturnsAsync(entityBaseList);

            var actual = await crudController.GetAll();

            mockStorage.Verify(c => c.GetAllEntitiesDto(), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].IsDeleted,
                    actual.Value[0].IsDeleted);
            });

        }
    }
}
