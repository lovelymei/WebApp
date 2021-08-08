using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IPerformers
    {
        Task<bool> AttachAlbum(Guid accountId, Guid albumId);
        Task<bool> AttachSong(Guid accountId, Guid songId);
        Task<List<Song>> GetAllPerformerSongs(Guid id);
    }
}