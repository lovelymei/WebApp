using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Song>> GetAllListenerSongs(Guid id)
        {
            var user = await _db.Listeners.Include(c => c.Songs).FirstOrDefaultAsync(c => c.ListenerId == id && c.IsDeleted == false);

            return user.Songs.ToList();
        }

        public async Task<bool> AttachSong(Guid accountId, Guid songId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.AccountId == accountId);

            var song = await _db.Songs.FirstOrDefaultAsync(c => c.SongId == songId);

            if (listener == null || song == null) return false;

            listener.Songs.Add(song);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<bool> AttachAlbum(Guid accountId, Guid albumId)
        {
            var listener = await _db.Listeners.FirstOrDefaultAsync(c => c.AccountId == accountId);

            var album = await _db.Albums.FirstOrDefaultAsync(c => c.AlbumId == albumId);

            if (listener == null || album == null) return false;

            listener.Albums.Add(album);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }

}
