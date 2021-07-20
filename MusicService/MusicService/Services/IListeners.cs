using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IListeners
    {
        Task<List<Song>> GetAllListenerSongs(Guid id);
        Task<bool> AttachSong(Guid accountId, Guid songId);
        Task<bool> AttachAlbum(Guid accountId, Guid albumId);
    }
}