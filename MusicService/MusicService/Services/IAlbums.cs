using MusicService.Models;
using System;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IAlbums
    {
        Task<Album> AddAlbum(string title);
        Task<bool> AttachMusicSong(Guid albumId, Guid songId);
    }
}