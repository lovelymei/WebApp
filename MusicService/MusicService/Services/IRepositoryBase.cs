using MusicService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IRepositoryBase<TDto> 
        where TDto : AccountBaseDto
    {
        /// <summary>
        /// Удаление 
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<bool> DeleteEntity(Guid id);

        /// <summary>
        /// Получение всех удаленных записей
        /// </summary>
        /// <returns></returns>
        Task<List<TDto>> GetAllDeletedEntities();

        /// <summary>
        /// Получение всех записей 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TDto>> GetAllEntities();

        /// <summary>
        /// Получение записи по идентификатору
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<TDto> GetEntity(Guid id);

        /// <summary>
        /// Восстановление записи
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<bool> RestoreEntity(Guid id);
    }
}