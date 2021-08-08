using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class PerformersInSQLRepository : RepositoryBase<Performer,PerformerDto> , IPerformers
    {
        MusicDatabase _db;

        public override DbSet<Performer> GetDbSet()
        {
            return _db.Performers;
        }
        public PerformersInSQLRepository(MusicDatabase db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Song>> GetAllPerformerSongs(Guid id)
        {
            var performer = await _db.Performers.Include(c => c.Songs).FirstOrDefaultAsync(c => c.AccountId == id && c.IsDeleted == false);

            return performer.Songs.ToList();
        }

        public async Task<bool> AttachSong(Guid accountId, Guid songId)
        {
            var performer = await _db.Performers.FirstOrDefaultAsync(c => c.AccountId == accountId);

            var song = await _db.Songs.FirstOrDefaultAsync(c => c.AccountId == songId);

            if (performer == null || song == null) return false;

            performer.Songs.Add(song);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> AttachAlbum(Guid accountId, Guid albumId)
        {
            var performer = await _db.Performers.FirstOrDefaultAsync(c => c.AccountId == accountId);

            var album = await _db.Albums.FirstOrDefaultAsync(c => c.AccountId == albumId);

            if (performer == null || album == null) return false;

            performer.Albums.Add(album);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }
}
