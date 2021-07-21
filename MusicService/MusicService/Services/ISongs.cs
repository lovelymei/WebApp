using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ISongs
    {
        /// <summary>
        /// Добавить песню
        /// </summary>
        /// <param name="title"> Название песни </param>
        /// <param name="duration"> Длительность </param>
        /// <returns></returns>
        Task<bool> AddSong(string title, long duration);

        /// <summary>
        /// Удалить песню
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <returns></returns>
        Task<bool> DeleteSong(Guid id);

        /// <summary>
        /// Получить все песни
        /// </summary>
        /// <returns></returns>
        Task<List<Song>> GetAllSongs();

        /// <summary>
        /// Получить песню
        /// </summary>
        /// <param name="id"> Идентификатор песни </param>
        /// <returns></returns>
        Task<Song> GetSong(Guid id);

        /// <summary>
        /// Получить песню по названию
        /// </summary>
        /// <param name="title"> Название </param>
        /// <returns></returns>
        Task<Song> GetSongByTitle(string title);
    }
}