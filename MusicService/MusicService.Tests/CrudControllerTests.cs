using Microsoft.AspNetCore.Mvc;
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
        private Guid _id = Guid.NewGuid();

        private List<EntityBaseDto> GetEntityList(bool isDeleted = false)
        { 
            var entities = new List<EntityBaseDto>()
            {
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()}),
                new EntityBaseDto(new EntityBase() { EntityId = Guid.NewGuid()})
            };

            if (isDeleted == false)
            {
                return entities;
            }
            else
            {
                entities.ForEach(c => c.IsDeleted = true);
                return entities;
            }
        }

        

        [Test]
        public async Task GetAll_Void_EntitiesList()
        {
           
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();

            var expected = GetEntityList(true);

            var entityBaseList = expected;

            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.GetAllEntitiesDto()).ReturnsAsync(entityBaseList);

            var actual = await crudController.GetAll();
           

            mockStorage.Verify(c => c.GetAllEntitiesDto(), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected[0].IsDeleted, actual.Value[0].IsDeleted);
            });

        }

        [Test]
        public async Task GetAllDeletedEntities_Void_DeletedEntitiesList()
        {
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();

            var expected = GetEntityList();

            var entityBaseDtoList = expected;

            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.GetAllDeletedEntitiesDto()).ReturnsAsync(entityBaseDtoList);

            var actual = await crudController.GetAllDeleted();

            mockStorage.Verify(c => c.GetAllDeletedEntitiesDto(), Times.Once);

            Assert.Multiple(() =>
            {
                for(int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].IsDeleted, actual.Value[i].IsDeleted);
                }
            });
        }


        [Test]
        public async Task Get_Id_Entity()
        {
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();

            var entityBase = new EntityBase();
            var entityBaseDto = new EntityBaseDto(entityBase);
            var expected = entityBaseDto;


            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.GetEntityDto(It.IsAny<Guid>())).ReturnsAsync(entityBaseDto);

            var actual = await crudController.Get(_id);

            mockStorage.Verify(c => c.GetEntityDto(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.IsDeleted, actual.Value.IsDeleted);
            });

        }

        [Test]
        public async Task Delete_Id_CodeResponse()
        {
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();
            var expected = new OkResult();

            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.DeleteEntity(It.IsAny<Guid>())).ReturnsAsync(true);

            var actual = await crudController.Delete(_id) as OkResult;

            mockStorage.Verify(c => c.DeleteEntity(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            });
        }
        

        [Test]
        public async Task Restore_Id_CodeResponse()
        {
            var mockStorage = new Mock<IStorage<EntityBaseDto>>();
            var expected = new OkResult();

            var crudController = new CrudControllerBase<EntityBaseDto>(mockStorage.Object);
            mockStorage.Setup(c => c.RestoreEntity(It.IsAny<Guid>())).ReturnsAsync(true);

            var actual = await crudController.Restore(_id) as OkResult;

            mockStorage.Verify(c => c.RestoreEntity(It.IsAny<Guid>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.StatusCode, actual.StatusCode);
            });

        }
    }
}
