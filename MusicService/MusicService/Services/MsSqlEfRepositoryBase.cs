using Microsoft.EntityFrameworkCore;
using MusicService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicService.Services
{
    //TODO: mapper 
    //MSSQLEFRepositoryBase
    public abstract class MsSqlEfRepositoryBase<TEntity, TDto> : IStorage<TDto> 
        where TEntity : EntityBase
        where TDto : EntityBaseDto
    {
        public abstract DbSet<TEntity> GetDbSet();
        private readonly MusicDatabase _db;

        protected MsSqlEfRepositoryBase(MusicDatabase db)
        {
            _db = db;
        }

        private static TDto TransformToDto(TEntity account)
        {
            //получаем тип TDto 
            Type type = typeof(TDto);

            //получаем открытый конструктор, в который надо передать объект типа EntityBase
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(EntityBase) });

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

        private async Task<IEnumerable<TEntity>> GetAllEntities()
        {
            await Task.CompletedTask;

            var collection = GetDbSet();

            return collection.Where(c => c.IsDeleted == false);    
        }

        public virtual async Task<IEnumerable<TDto>> GetAllEntitiesDto()
        {
            var collection = await GetAllEntities();

            return collection.Select(c=>TransformToDto(c));
        }

        private async Task<TEntity> GetEntity(Guid id)
        {
            var collection = await GetAllEntities(); 

            var entity =  collection.FirstOrDefault(c => c.EntityId == id && c.IsDeleted == false);

            return entity;
        }

        public virtual async Task<TDto> GetEntityDto(Guid id)
        {
            var collection = await GetAllEntities();

            var entity = collection.FirstOrDefault(c => c.EntityId == id && c.IsDeleted == false);

            return TransformToDto(entity);
        }

        public virtual async Task<bool> DeleteEntity(Guid id)
        {
            var item = await GetEntity(id);

            if (item == null) return false;

            item.IsDeleted = true;

            
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllDeletedEntitiesDto()
        {
            var collection = await GetAllDeletedEntities();

            return collection.Select(c => TransformToDto(c));
        }

        private async Task<IEnumerable<TEntity>> GetAllDeletedEntities()
        {
            await Task.CompletedTask;

            var collection =  GetDbSet();

            return collection.Where(c => c.IsDeleted == true);
        }

        public virtual async Task<bool> RestoreEntity(Guid id)
        {
            var collection = await GetAllDeletedEntities();

            var item = collection.FirstOrDefault(c => c.EntityId == id);

            if (item == null) return false;

            item.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }
}
