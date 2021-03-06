using MusicService.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IStorage<TDto> 
        where TDto : EntityBaseDto
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
        Task<IEnumerable<TDto>> GetAllDeletedEntitiesDto();

        /// <summary>
        /// Получение всех записей 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TDto>> GetAllEntitiesDto();

        /// <summary>
        /// Получение записи по идентификатору
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<TDto> GetEntityDto(Guid id);

        /// <summary>
        /// Восстановление записи
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<bool> RestoreEntity(Guid id);

    }
}