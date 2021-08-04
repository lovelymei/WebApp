using EntitiesLibrary;
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
    public abstract class CrudControllerBase<T> : ControllerBase where T: AccountBase
    {
        protected readonly ICrud<T> _crud;

        protected CrudControllerBase(ICrud<T> crud) 
        {
            _crud = crud;
        }


        /// <summary>
        /// Получить все 
        /// </summary>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> не найдены </response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TDto>>> GetAll()
        {
            var items = await _crud.GetAllEntities();
            List<TDto> itemsDto = new List<TDto>();
            foreach (var item in items)
            {
                itemsDto.Add(new TDto(item));
            }

            return Ok(itemsDto);
        }

        /// <summary>
        /// Получить  по идентификатору
        /// </summary>
        /// <param name="id"> Идентификатор </param>
        /// <response code="200"> Успешно </response>
        /// <response code="404"> не найден</response>
        /// <response code="500"> Ошибка сервера </response>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AlbumDto>> Get(Guid id)
        {
            var album = await _albums.GetAlbum(id);

            if (album == null) return NotFound();

            return new AlbumDto(album);
        }
    }


    /// <summary>
    /// Удалить 
    /// </summary>
    /// <param name="id"> Идентификатор </param>
    /// <returns></returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var isDeleted = await _albums.DeleteAlbum(id);
        return isDeleted ? Ok() : NotFound();
    }

    /// <summary>
    /// Восстановить
    /// </summary>
    /// <param name="id"> Идентификатор </param>
    /// <response code="200"> Успешно</response>
    /// <response code="404"> не найден</response>
    /// <response code="500"> Ошибка сервера </response>
    /// <returns></returns>
    [HttpPut("isDeleted/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Restore(Guid id)
    {
        var isRestored = await _albums.RestoreAlbum(id);

        return isRestored ? Ok() : NotFound(new { errorMessage = "Проверьте id" });
    }

    /// <summary>
    /// Получить все удаленные 
    /// </summary>
    ///  <response code="200"> Успешно</response>
    ///  <response code="404"> Не найдено ни одного удаленного </response>
    ///   <response code="500"> Ошибка сервера </response>
    /// <returns></returns>
    [HttpGet("isDeleted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<AlbumDto>>> GetAllDeleted()
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
