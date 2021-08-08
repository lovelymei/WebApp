using EntitiesLibrary;
using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class AlbumsInSQLRepository : RepositoryBase<Album, AlbumDto>, IAlbums
    {
        private readonly MusicDatabase _db;

        public AlbumsInSQLRepository(MusicDatabase db) : base(db)
        {
            _db = db;
        }

 
        public async Task<Album> AddAlbum(string title)
        {
            var random = new Random();
            var newAlbum = new Album()
            {
                Name = title,
                ProductionDate = DateTime.MinValue.Add(TimeSpan.FromTicks((long)(random.NextDouble() * DateTime.MaxValue.Ticks)))
            };

            await _db.Albums.AddAsync(newAlbum);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return newAlbum;
        }


        public async Task<bool> AttachMusicSong(Guid albumId, Guid songId)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(c => c.AccountId == albumId);

            var song = await _db.Songs.FirstOrDefaultAsync(c => c.AccountId == songId);

            if (album == null || song == null) return false;

            album.Songs.Add(song);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

    }
}
