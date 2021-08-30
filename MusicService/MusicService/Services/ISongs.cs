using Microsoft.EntityFrameworkCore;
using MusicService.Models;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ISongs
    {
        Task<Song> AddSong(string title, long duration);
        DbSet<Song> GetDbSet();
        Task<Song> GetSongByTitle(string title);
    }
}