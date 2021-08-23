using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformerController : ControllerBase
    {
        IPerformers _performers;
        IMapper _mapper;

        public PerformerController(IPerformers performers, IMapper mapper)

        {
            _mapper = mapper;
            _performers = performers;
        }

        /// <summary>
        /// Получить все песни исполнителя
        /// </summary>
        /// <returns></returns>
        [HttpGet("songs")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SongDto>>> GetAllPerformersSongs()
        {
            var performerId = User.GetAccountId();
            var songs = await _performers.GetAllPerformerSongs(performerId);
            return Ok(songs);
           
        }


        /// <summary>
        /// Получить все альбомы исполнителя
        /// </summary>
        /// <returns></returns>
        [HttpGet("albums")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SongDto>>> GetAllPerformersAlbums()
        {
            var performerId = User.GetAccountId();
            var albums = await _performers.GetAllPerformerAlbums(performerId);
            return Ok(albums);

        }

        /// <summary>
        /// Прикрепить песню к исполнителю
        /// </summary>
        /// <param name="songId"> Идентификатор песни</param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден исполнитель или песня </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("AttachMusicSongToPerformer/{songId}")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AttachMusicSongToPerformer(Guid songId)
        {
            var performerId = User.GetAccountId();
            var isAttached = await _performers.AttachSong(songId, performerId);
            return isAttached ? Ok() : NotFound();
        }

        /// <summary>
        /// Прикрепить альбом к исполнителю
        /// </summary>
        /// <param name="albumId"> Идентификатор альбома</param>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> Не найден исполнитель или альбом </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("AttachAlbumToPerformer/{albumId}")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AttachAlbumToPerformer(Guid albumId)
        {
            var performerId = User.GetAccountId();
            var isAttached = await _performers.AttachAlbum(albumId, performerId);
            return isAttached ? Ok() : NotFound();
        }

    }
}
