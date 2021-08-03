using EntitiesLibrary;
using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public abstract class RepositoryBase<T> : ICrud<T> where T : AccountBase
    {
        private readonly MusicDatabase _db;

        protected RepositoryBase(MusicDatabase db)
        {
            _db = db;
        }

        public async Task<List<T>> GetAllEntities()
        {
            await Task.CompletedTask;
            var collection = (DbSet<T>)_db.GetCollection<T>();
            return collection.Where(c => c.IsDeleted == false).ToList();
        }

        public async Task<T> GetEntity(Guid id)
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

        public async Task<List<T>> GetAllDeletedEntities()
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
