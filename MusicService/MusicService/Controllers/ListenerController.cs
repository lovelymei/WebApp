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
    public class ListenerController : CrudControllerBase<ListenerDto>
    {
        private readonly IListeners _listeners;
        public ListenerController(IRepositoryBase<ListenerDto> listeners) : base(listeners)
        {

        }

        /// <summary>
        /// Получить все песни пользователя
        /// </summary>
        /// <param name="id"> Идентификатор пользователя </param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Слушатель не найден </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("GetSongs/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден слушатель или песня</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("{listenerId}/{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден слушатель / альбом </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("{listenerId}/{albumId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AttachAlbumToListener(Guid listenerId, Guid albumId)
        {
            var isAttached = await _listeners.AttachAlbum(listenerId, albumId);
            return isAttached ? Ok() : NotFound();
        }
    }
}
