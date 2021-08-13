﻿using Microsoft.EntityFrameworkCore;
using NewEntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicService.Services
{
    //TODO: AccountBase - EntityBase
    //TODO: AccountBaseDto replace to EntityLibrary
    //TODO: mapper 
    //MSSQLEFRepositoryBase
    public abstract class RepositoryBase<TEntity, TDto> : IStorage<TDto> 
        where TEntity : EntityBase
        where TDto : EntityBaseDto
    {
        private readonly MusicDatabase _db;

        protected RepositoryBase(MusicDatabase db)
        {
            _db = db;
        }

        public abstract DbSet<TEntity> GetDbSet();

        private TDto TransformToDto(TEntity account)
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


        public virtual async Task<IEnumerable<TDto>> GetAllEntities()
        {
            await Task.CompletedTask;

            var collection = GetDbSet();

            return collection
                .Where(c => c.IsDeleted == false)
                .Select(c => TransformToDto(c));
        }

        public virtual async Task<TDto> GetEntity(Guid id)
        {
            var collection = await GetAllEntities(); 

            return collection.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
        }

        public virtual async Task<bool> DeleteEntity(Guid id)
        {
            var item = await GetEntity(id);

            if (item == null) return false;

            item.IsDeleted = true;

            return true;
        }

        public virtual async Task<List<TDto>> GetAllDeletedEntities()
        {
            var collection = await GetAllEntities();

            return collection.Where(c => c.IsDeleted == true).ToList();
        }

        public virtual async Task<bool> RestoreEntity(Guid id)
        {
            var collection = await GetAllDeletedEntities();
            var item = collection.FirstOrDefault(c => c.Id == id && c.IsDeleted == true);
            item.IsDeleted = false;
            return true;
        }
    }
}
