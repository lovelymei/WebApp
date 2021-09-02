using AuthorizationService.Extensions;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dto;
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
        /// Получить все песни слушателя
        /// </summary>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Слушатель не найден </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("songs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.listener)]
        public async Task<ActionResult<List<SongDto>>> GetAllListenerSongs()
        {
            var listenerId = User.GetAccountId();
            var songs = await _listeners.GetAllListenerSongs(listenerId);
            return songs.ToList();
        }


        /// <summary>
        /// Получить все альбомы слушателя
        /// </summary>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Слушатель не найден </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("albums")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.listener)]
        public async Task<ActionResult<List<AlbumDto>>> GetAllListenerAlbums()
        {
            var listenerId = User.GetAccountId();
            var albums = await _listeners.GetAllListenerAlbums(listenerId);
            return albums.ToList();
        }

        /// <summary>
        /// Прикрепить песню к слушателю 
        /// </summary>
        /// <param name="songId"> Идентификатор песни </param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден слушатель или песня</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("AttachSongToListener/{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.listener)]
        public async Task<ActionResult> AttachSongToListener(Guid songId)
        {
            var listenerId = User.GetAccountId();
            var isAttached = await _listeners.AttachSong(songId, listenerId);
            return isAttached ? Ok() : NotFound();
        }

        /// <summary>
        /// Прикрепить альбом к слушателю
        /// </summary>
        /// <param name="albumId"> Идентификатор альбома </param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден слушатель / альбом </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("AttachAlbumToListener/{albumId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.listener)]
        public async Task<ActionResult> AttachAlbumToListener(Guid albumId)
        {
            var listenerId = User.GetAccountId();
            var isAttached = await _listeners.AttachAlbum(albumId, listenerId);
            return isAttached ? Ok() : NotFound();
        }
    }
}
