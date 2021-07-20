using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class AlbumsInSQLRepository : IAlbums
    {
        private readonly MusicDatabase _db;

        public AlbumsInSQLRepository(MusicDatabase db)
        {
            _db = db;
        }

        public async Task<List<Album>> GetAllAlbums()
        {
            await Task.CompletedTask;
            return _db.Albums.Where(c => c.IsDeleted == false).ToList();
        }
        public async Task<Album> GetAlbum(Guid id)
        {
            var albums = await _db.Albums.ToListAsync();

            var album = albums.FirstOrDefault(c => c.AlbumId == id && c.IsDeleted == false);

            if (album == null) return null;

            return album;
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

        public async Task<bool> DeleteAlbum(Guid id)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(c => c.AlbumId == id);

            if (album == null) return false;

            album.IsDeleted = true;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<bool> AttachMusicSong(Guid albumId, Guid songId)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(c => c.AlbumId == albumId);

            var song = await _db.Songs.FirstOrDefaultAsync(c => c.SongId == songId);

            if (album == null || song == null) return false;

            album.Songs.Add(song);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<List<Album>> GetAllDeletedAlbums()
        {
            await Task.CompletedTask;
            return _db.Albums.Where(a => a.IsDeleted == true).ToList();
        }

        public async Task<bool> RestoreAlbum(Guid id)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null) return false;

            album.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }
    }
}
