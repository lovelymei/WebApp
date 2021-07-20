using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongs _songs;

        public SongController(ISongs songs)
        {
            _songs = songs;
        }

        /// <summary>
        /// Получить все песни
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<SongDto>>> GetAllSongs()
        {
            var songs = await _songs.GetAllSongs();
            List<SongDto> songsDto = new List<SongDto>();

            foreach (var song in songs)
            {
                songsDto.Add(new SongDto(song));
            }

            return Ok(songsDto);
        }

        /// <summary>
        /// Получить песню
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SongDto>> GetSong(Guid id)
        {
            var song = await _songs.GetSong(id);

            if (song == null) return NotFound();

            return new SongDto(song);
        }

        /// <summary>
        /// Добавить новую песню
        /// </summary>
        /// <param name="title">Название</param>
        /// <param name="duration">Длительность</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddSong(string title, long duration)
        {
            var isAdded = await _songs.AddSong(title, duration);
            return isAdded ? Ok() : NotFound();
        }

        /// <summary>
        /// Удалить песню
        /// </summary>
        /// <param name="id"> Идентификатор песни</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSong(Guid id)
        {
            var isDeleted = await _songs.DeleteSong(id);
            return isDeleted ? Ok() : NotFound();
        }

    }
}
