using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.Dto;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbums _albums;

        public AlbumController(IAlbums albums)
        {
            _albums = albums;
        }


        /// <summary>
        /// Получить все альбомы
        /// </summary>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> Альбомы не найдены </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AlbumDto>>> GetAllAlbums()
        {
            var albums = await _albums.GetAllAlbums();
            List<AlbumDto> albumsDto = new List<AlbumDto>();

            foreach (var album in albums)
            {
                albumsDto.Add(new AlbumDto(album));
            }

            return Ok(albumsDto);
        }

        /// <summary>
        /// Получить альбом по идентификатору
        /// </summary>
        /// <param name="id"> Идентификатор альбома</param>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> Альбом не найден</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AlbumDto>> GetAlbum(Guid id)
        {
            var album = await _albums.GetAlbum(id);

            if (album == null) return NotFound();

            return new AlbumDto(album);
        }


        /// <summary>
        /// Добавить альбом
        /// </summary>
        /// <param name="title"> Название альбома </param>
        /// <response code="200"> Успешно</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AlbumDto>> AddAlbum(string title)
        {
            var album = await _albums.AddAlbum(title);
            return new AlbumDto(album);

        }

        /// <summary>
        /// Удалить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAlbum(Guid id)
        {
            var isDeleted = await _albums.DeleteAlbum(id);
            return isDeleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Восстановить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <response code="200"> Успешно</response>
        /// <response code="404"> Альбом не найден</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpPut("isDeleted/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RestoreAlbum(Guid id)
        {
            var isRestored = await _albums.RestoreAlbum(id);

            return isRestored ? Ok() : NotFound(new { errorMessage = "Проверьте id" });
        }

        /// <summary>
        /// Получить все удаленные альбомы
        /// </summary>
        ///  <response code="200"> Успешно</response>
        ///  <response code="404"> Не найдено ни одного удаленного альбома</response>
        ///   <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("isDeleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AlbumDto>>> GetAllDeletedAlbums()
        {
            var albums = await _albums.GetAllDeletedAlbums();
            List<AlbumDto> albumsDto = new List<AlbumDto>();

            foreach (var album in albums)
            {
                albumsDto.Add(new AlbumDto(album));
            }

            return Ok(albumsDto);
        }
    }
}
