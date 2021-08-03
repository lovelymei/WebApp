using EntitiesLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ICrud<T> where T : AccountBase
    {
        Task<bool> DeleteEntity(Guid id);
        Task<List<T>> GetAllDeletedEntities();
        Task<List<T>> GetAllEntities();
        Task<T> GetEntity(Guid id);
        Task<bool> RestoreEntity(Guid id);
    }
}