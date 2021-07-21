using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IListeners
    {
        /// <summary>
        /// Получить все песни слушателя
        /// </summary>
        /// <param name="id"> Идентификатор слушателя </param>
        /// <returns></returns>
        Task<List<Song>> GetAllListenerSongs(Guid id);

        /// <summary>
        /// Прикрепить песню
        /// </summary>
        /// <param name="accountId"> Идентификатор слушателя </param>
        /// <param name="songId"> Идентификатор песни </param>
        /// <returns></returns>
        Task<bool> AttachSong(Guid accountId, Guid songId);

        /// <summary>
        /// Прикрепить альбом
        /// </summary>
        /// <param name="accountId"> Идентификатор слушателя </param>
        /// <param name="albumId"> Идентификатор альбома </param>
        /// <returns></returns>
        Task<bool> AttachAlbum(Guid accountId, Guid albumId);
    }
}