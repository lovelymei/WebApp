using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ISongs
    {
        Task<bool> AddSong(string title, long duration);
        Task<bool> DeleteSong(Guid id);
        Task<List<Song>> GetAllSongs();
        Task<Song> GetSong(Guid id);
        Task<Song> GetSongByTitle(string title);
    }
}