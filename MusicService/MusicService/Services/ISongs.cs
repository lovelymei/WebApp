using MusicService.Models;
using System.Threading.Tasks;

namespace MusicService.Services
{
    public interface ISongs
    {
        Task<bool> AddSong(string title, long duration);
        Task<Song> GetSongByTitle(string title);
    }
}