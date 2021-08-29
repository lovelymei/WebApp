using Microsoft.EntityFrameworkCore;
using MusicService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicService.Services
{
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

        protected abstract TDto TransformToDto(TEntity account);

        public virtual async Task<IEnumerable<TDto>> GetAllEntitiesDto()
        {
            await Task.CompletedTask;

            return GetDbSet()
                .Where(c => c.IsDeleted == false)
                .Select(c=>TransformToDto(c));
        }

        public virtual async Task<TDto> GetEntityDto(Guid id)
        {
            await Task.CompletedTask;

            var entity = GetDbSet().FirstOrDefault(c => c.EntityId == id && c.IsDeleted == false);

            return TransformToDto(entity);
        }

        public virtual async Task<bool> DeleteEntity(Guid id)
        {
            var item = GetDbSet()
                .FirstOrDefault(c => c.EntityId == id && c.IsDeleted == false);

            if (item == null) return false;

            item.IsDeleted = true;
            
            await _db.SaveChangesAsync();

            return true;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllDeletedEntitiesDto()
        {
            await Task.CompletedTask;

            var collection = GetDbSet();

            return collection
                .Where(c => c.IsDeleted == true)
                .Select(c => TransformToDto(c));
        }

        public virtual async Task<bool> RestoreEntity(Guid id)
        {
            await Task.CompletedTask;

            var collection = GetDbSet();

            var item = collection.FirstOrDefault(c => c.EntityId == id && c.IsDeleted == true);

            if (item == null) return false;

            item.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }
}
