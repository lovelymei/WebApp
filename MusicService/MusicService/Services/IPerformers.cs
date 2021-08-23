using MusicService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface IPerformers
    {
        Task<bool> AttachAlbum(Guid albumId, Guid performerId);
        Task<bool> AttachSong(Guid songId, Guid performerId);
        Task<IEnumerable<AlbumDto>> GetAllPerformerAlbums(Guid performerId);
        Task<IEnumerable<SongDto>> GetAllPerformerSongs(Guid performerId);
    }
}