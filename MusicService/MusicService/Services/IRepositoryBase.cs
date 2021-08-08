using EntitiesLibrary;
using MusicService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IRepositoryBase<T,TDto> 
        where T : AccountBase
        where TDto: AccountBaseDto
    {
        Task<bool> DeleteEntity(Guid id);
        Task<List<T>> GetAllDeletedEntities();
        Task<List<T>> GetAllEntities();
        Task<T> GetEntity(Guid id);
        Task<bool> RestoreEntity(Guid id);
    }
}