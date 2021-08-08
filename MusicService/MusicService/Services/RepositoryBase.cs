using EntitiesLibrary;
using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public abstract class RepositoryBase<TEntity,TDto> : IRepositoryBase<TEntity, TDto> 
        where TEntity : AccountBase
        where TDto: AccountBaseDto
    {
        private readonly MusicDatabase _db;

        protected RepositoryBase(MusicDatabase db)
        {
            _db = db;
        }


        private TDto TransformToDto(TEntity account)
        {
            //получаем тип TDto 
            Type type = typeof(TDto);

            //получаем открытый конструктор, в который надо передать объект типа AccountBase
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(AccountBase) });

            if (constructorInfo != null)
            {
                try
                {
                    return (TDto)Activator.CreateInstance(typeof(TDto), account);
                }
                catch (Exception)
                {
                    //у заданного типа нет нужного конструктора
                    throw new NotSupportedException();
                }
            }
            throw new NotSupportedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllEntities()
        {
            //public abstract GetDbset()
            await Task.CompletedTask;
            var collection = (DbSet<TEntity>)_db
                .GetCollection<TEntity>();


            return (IEnumerable<TEntity>)collection
                .Where(c => c.IsDeleted == false)
                .Select(c=> TransformToDto(c));
        }

        public async Task<TEntity> GetEntity(Guid id)
        {
            var collection = await GetAllEntities();
            var item = collection.FirstOrDefault(c => c.AccountId == id && c.IsDeleted == false);
            return item;
        }

        public async Task<bool> DeleteEntity(Guid id)
        {
            var item = await GetEntity(id);

            if (item == null) return false;

            item.IsDeleted = true;
            return true;
        }

        public async Task<List<TEntity>> GetAllDeletedEntities()
        {
            var collection = await GetAllEntities();
            return collection.Where(c => c.IsDeleted == true).ToList();
        }

        public async Task<bool> RestoreEntity(Guid id)
        {
            var collection = await GetAllDeletedEntities();
            var item = collection.FirstOrDefault(c => c.AccountId == id && c.IsDeleted == true);
            item.IsDeleted = false;
            return true;
        }

    }
}
