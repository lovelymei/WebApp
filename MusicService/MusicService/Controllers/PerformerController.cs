using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformerController : CrudControllerBase<PerformerDto>
    {
        IPerformers _performers;

        public PerformerController(IPerformers performers, IStorage<PerformerDto> crud) : base(crud)
        {
            _performers = performers;
        }

        /// <summary>
        /// Получить все песни исполнителя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SongDto>>> GetAllPerformersSongs(Guid id)
        {
            var songs = await _performers.GetAllPerformerSongs(id);
            List<SongDto> songsDto = new List<SongDto>();
            foreach (var song in songs)
            {
                songsDto.Add(new SongDto(song));
            }

            return Ok(songsDto);
        }

        /// <summary>
        /// Прикрепить песню к исполнителю
        /// </summary>
        /// <param name="performerId"> Идентификатор исполнителя</param>
        /// <param name="songId"> Идентификатор песни</param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Не найден исполнитель или песня </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("{performerId}/{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AttachMusicSongToPerformer(Guid performerId, Guid songId)
        {
            var isAttached = await _performers.AttachSong(performerId, songId);
            return isAttached ? Ok() : NotFound();
        }

        /// <summary>
        /// Прикрепить альбом к исполнителю
        /// </summary>
        /// <param name="performerId"> Идентификатор исполнителя</param>
        /// <param name="albumId"> Идентификатор альбома</param>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> Не найден исполнитель или альбом </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("{performerId}/{albumId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AttachAlbumToPerformer(Guid performerId, Guid albumId)
        {
            var isAttached = await _performers.AttachAlbum(performerId, albumId);
            return isAttached ? Ok() : NotFound();
        }

    }
}
