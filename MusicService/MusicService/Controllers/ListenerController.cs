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
    public class ListenerController : ControllerBase
    {
        private readonly IListeners _listeners;
        public ListenerController(IListeners listeners)
        {
            _listeners = listeners;
        }

        /// <summary>
        /// Получить все песни пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSongs/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<SongDto>>> GetAllUsersSongs(Guid id)
        {
            var songs = await _listeners.GetAllListenerSongs(id);
            List<SongDto> songsDto = new List<SongDto>();
            foreach (var song in songs)
            {
                songsDto.Add(new SongDto(song));
            }

            return Ok(songsDto);
        }

        /// <summary>
        /// Прикрепить песню к слушателю 
        /// </summary>
        /// <param name="listenerId"> Идентификатор слушателя </param>
        /// <param name="songId"> Идентификатор песни </param>
        /// <returns></returns>
        [HttpPut("{listenerId}/{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AttachSongToListener(Guid listenerId, Guid songId)
        {
            var isAttached = await _listeners.AttachSong(listenerId, songId);
            return isAttached ? Ok() : NotFound();
        }

        /// <summary>
        /// Прикрепить альбом к слушателю
        /// </summary>
        /// <param name="listenerId"> Идентификатор слушателя </param>
        /// <param name="albumId"> Идентификатор альбома </param>
        /// <returns></returns>
        [HttpPut("{listenerId}/{albumId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AttachAlbumToListener(Guid listenerId, Guid albumId)
        {
            var isAttached = await _listeners.AttachAlbum(listenerId, albumId);
            return isAttached ? Ok() : NotFound();
        }
    }
}
