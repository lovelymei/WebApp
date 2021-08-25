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

        private static TDto TransformToDto(TEntity account)
        {
            //получаем тип TDto 
            Type type = typeof(TDto);

            //получаем открытый конструктор, в который надо передать объект типа AccountBase
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

            return collection
                .Where(c => c.IsDeleted == false);
                
        }

        public virtual async Task<IEnumerable<TDto>> GetAllEntitiesDto()
        {
            var collection = await GetAllEntities();

            List<TDto> result = null;

            foreach (var item in collection)
            {
                result.Add(TransformToDto(item));
            }
            return result;
        }

        public virtual async Task<TDto> GetEntity(Guid id)
        {
            var collection = await GetAllEntities(); 
            

            var entity =  collection.FirstOrDefault(c => c.EntityId == id && c.IsDeleted == false);

            return TransformToDto(entity);
        }

        public virtual async Task<bool> DeleteEntity(Guid id)
        {
            var item = await GetEntity(id);

            if (item == null) return false;

            item.IsDeleted = true;

            return true;
        }

        public virtual async Task<List<TDto>> GetAllDeletedEntitiesDto()
        {
            var collection = await GetAllDeletedEntities();

            List<TDto> resultCollection = null;

            foreach (var item in collection)
            {
                resultCollection.Add(TransformToDto(item));
            }

            return resultCollection;
        }

        private async Task<List<TEntity>> GetAllDeletedEntities()
        {
            var collection = await GetAllEntities();
            return collection.Where(c => c.IsDeleted == true).ToList();
        }

        public virtual async Task<bool> RestoreEntity(Guid id)
        {
            var collection = await GetAllDeletedEntities();
            var item = collection.FirstOrDefault(c => c.EntityId == id && c.IsDeleted == true);
            item.IsDeleted = false;
            return true;
        }
    }
}
