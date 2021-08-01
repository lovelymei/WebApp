using EntitiesLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ICrud<T> where T : AccountBase
    {
        /// <summary>
        /// Удаление по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdEntity(Guid id);

        /// <summary>
        /// Получение всех удаленных сущностей
        /// </summary>
        /// <returns></returns>
        Task<DbSet<T>> GetAllDeletedEntities();
        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <returns></returns>
        Task<DbSet<T>> GetAllEntities();

        /// <summary>
        /// Получение сущности по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdEntity(Guid id);

        /// <summary>
        /// Восстановление сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RestoreEntity(Guid id);
    }
}