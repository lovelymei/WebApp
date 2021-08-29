using AuthorizationService.Extensions;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class AlbumController : CrudControllerBase<AlbumDto>
    {
        private readonly IAlbums _albums;

        public AlbumController(IAlbums albums, IStorage<AlbumDto> crud) : base(crud)
        {
            _albums = albums;
        }


        /// <summary>
        /// Добавить альбом
        /// </summary>
        /// <param name="title"> Название альбома </param>
        /// <response code="200"> Успешно</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator, Roles.performer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AlbumDto>> AddAlbum(string title)
        {
            var album = await _albums.AddAlbum(title);
            return new AlbumDto(album);
        }

    }
}
