using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IListeners
    {
        Task<bool> AttachAlbum(Guid albumId, Guid listenerId);
        Task<bool> AttachSong(Guid songId, Guid listenerId);
        Task<IEnumerable<AlbumDto>> GetAllListenerAlbums(Guid listenerId);
        Task<IEnumerable<SongDto>> GetAllListenerSongs(Guid listenerId);
    }
}