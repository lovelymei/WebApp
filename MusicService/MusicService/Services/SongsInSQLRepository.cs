using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public class SongsInSQLRepository : MsSqlEfRepositoryBase<Song, SongDto>, ISongs
    {
        private readonly MusicDatabase _db;
        public SongsInSQLRepository(MusicDatabase db) : base(db)
        {
            _db = db;
        }

        public override DbSet<Song> GetDbSet()
        {
            return _db.Songs;
        }

        public async Task<Song> GetSongByTitle(string title)
        {
            var songs = await _db.Songs.ToListAsync();

            var song = songs.FirstOrDefault(c => c.Title.Contains(title) && c.IsDeleted == false);

            if (song == null) return null;

            return song;
        }

        public async Task<Song> AddSong(string title, long duration)
        {
            var random = new Random();
            var newSong = new Song()
            {
                Title = title,
                DurationMs = duration,
                ProductionDate = DateTime.MinValue.Add(TimeSpan.FromTicks((long)(random.NextDouble() * DateTime.MaxValue.Ticks)))
            };

            await _db.Songs.AddAsync(newSong);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return newSong;
        }

        protected override SongDto TransformToDto(Song entity)
        {
            return new SongDto(entity);
        }
    }
}
