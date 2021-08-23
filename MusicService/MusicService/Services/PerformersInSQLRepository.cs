using AuthorizationService.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class PerformersInSQLRepository : IPerformers
    {
        private readonly MusicDatabase _db;

        public PerformersInSQLRepository(MusicDatabase db)
        {
            _db = db;

        }

        public async Task<IEnumerable<AlbumDto>> GetAllPerformerAlbums(Guid performerId)
        {
            await Task.CompletedTask;
            return _db.Albums
                .Where(c => c.PerformerId == performerId && c.IsDeleted == false)
                .Select(c => new AlbumDto(c));
        }

        public async Task<IEnumerable<SongDto>> GetAllPerformerSongs(Guid performerId)
        {
            await Task.CompletedTask;
            return _db.Songs
                .Where(c => c.PerformerId == performerId && c.IsDeleted == false)
                .Select(c => new SongDto(c));
        }

        public async Task<bool> AttachSong(Guid songId, Guid performerId)
        {
            var song = await _db.Songs.FirstOrDefaultAsync(c => c.EntityId == songId);

            if (song == null) return false;

            song.PerformerId = performerId;
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> AttachAlbum(Guid albumId, Guid performerId)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(c => c.EntityId == albumId);

            if (album == null) return false;

            album.PerformerId = performerId;
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }
}
