using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IAlbums 
    {
        Task<Album> AddAlbum(string title);
        Task<bool> AttachMusicSong(Guid albumId, Guid songId);
        Task<List<Album>> GetAllAlbums();
        Task<Album> GetAlbum(Guid id);
        Task<bool> DeleteAlbum(Guid id);
    }
}