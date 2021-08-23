using AuthorizationService.Extensions;
using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class ListenersInSQLRepository : IListeners
    {
        MusicDatabase _db;

        public ListenersInSQLRepository(MusicDatabase db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AlbumDto>> GetAllListenerAlbums(Guid listenerId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.EntityId == listenerId);

            if (listener == null)
            {
                listener = await CreateListener(listenerId);
                return listener.Albums.Select(c => new AlbumDto(c));
            }

            return listener.Albums.Select(c => new AlbumDto(c));


        }

        public async Task<IEnumerable<SongDto>> GetAllListenerSongs(Guid listenerId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.EntityId == listenerId);

            if (listener == null)
            {
                listener = await CreateListener(listenerId);
                return listener.Songs.Select(c => new SongDto(c));
            }

            return listener.Songs.Select(c => new SongDto(c));
        }

        private async Task<Listener> CreateListener(Guid listenerId)
        {
            if (listenerId == Guid.Empty) throw new Exception();

            var listener = new Listener()
            {
                EntityId = listenerId,
            };

            await _db.Listeners.AddAsync(listener);
            await _db.SaveChangesAsync();

            return listener;
        }

        public async Task<bool> AttachSong(Guid songId, Guid listenerId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.EntityId == listenerId);

            if (listener == null)
            {
                listener = await CreateListener(listenerId);
            }

            var song = await _db.Songs.FirstOrDefaultAsync(c => c.EntityId == songId);

            if (song == null) return false;

            listener.Songs.Add(song);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> AttachAlbum(Guid albumId, Guid listenerId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.EntityId == listenerId);

            if (listener == null)
            {
                listener = await CreateListener(listenerId);
            }

            var album = await _db.Albums.FirstOrDefaultAsync(c => c.EntityId == albumId);

            if (album == null) return false;

            listener.Albums.Add(album);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }

}
