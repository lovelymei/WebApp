using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dto;
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
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        public async Task<ActionResult> DeleteAlbum(Guid id)
        {
            var isDeleted = await _albums.DeleteAlbum(id);
            return isDeleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Восстановить альбом
        /// </summary>
        /// <param name="id"> Идентификатор альбома </param>
        /// <returns></returns>
        [HttpPut("isDeleted/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreAlbum(Guid id)
        {
            var isRestored = await _albums.RestoreAlbum(id);

            return isRestored ? Ok() : NotFound(new { errorMessage = "Проверьте id" });
        }

        /// <summary>
        /// Получить все удаленные альбомы
        /// </summary>
        /// <returns></returns>
        [HttpGet("isDeleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
