using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IAlbums
    {
        /// <summary>
        /// Добавить альбом
        /// </summary>
        /// <param name="title"> Название</param>
        /// <returns></returns>
        Task<Album> AddAlbum(string title);

        /// <summary>
        /// Прикрепить песню к альбому
        /// </summary>
        /// <param name="albumId"> Идентификатор альбома </param>
        /// <param name="songId"> Идентификатор песни </param>
        /// <returns></returns>
        Task<bool> AttachMusicSong(Guid albumId, Guid songId);

        /// <summary>
        /// Удалить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <returns></returns>
        Task<bool> DeleteAlbum(Guid id);

        /// <summary>
        /// Получить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <returns></returns>
        Task<Album> GetAlbum(Guid id);

        /// <summary>
        /// Получить все альбомы
        /// </summary>
        /// <returns></returns>
        Task<List<Album>> GetAllAlbums();

        /// <summary>
        /// Получить все удаленные альбомы
        /// </summary>
        /// <returns></returns>
        Task<List<Album>> GetAllDeletedAlbums();

        /// <summary>
        /// Восстановить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <returns></returns>
        Task<bool> RestoreAlbum(Guid id);
    }
}