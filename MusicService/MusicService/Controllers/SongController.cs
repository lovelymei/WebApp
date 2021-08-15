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
    public class SongController : CrudControllerBase<SongDto>
    {
        private readonly ISongs _songs;

        public SongController(ISongs songs, IStorage<SongDto> crud) : base(crud)
        {
            _songs = songs;
        }

        /// <summary>
        /// Добавить новую песню
        /// </summary>
        /// <param name="title">Название</param>
        /// <param name="duration">Длительность</param>
        /// <response code="200"> Успешно</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddSong(string title, long duration)
        {
            var isAdded = await _songs.AddSong(title, duration);
            return isAdded ? Ok() : NotFound();
        }
    }
}
