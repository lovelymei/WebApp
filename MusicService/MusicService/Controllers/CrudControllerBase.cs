using EntitiesLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicService.Controllers
{

    public abstract class CrudControllerBase<TEntity,TDto>
        : ControllerBase
        where TEntity : AccountBase
        where TDto : AccountBaseDto, new()
    {
        protected readonly IRepositoryBase<TEntity,TDto> _crud;

        protected CrudControllerBase(IRepositoryBase<TEntity,TDto> crud)
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
            return Ok(await _crud.GetAllEntities());
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
            var isDeleted = await _crud.DeleteEntity(id);

            return isDeleted ? Ok() : NotFound();
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
        public async Task<ActionResult<TDto>> Get(Guid id)
        {
            var entity = await _crud.GetEntity(id);

            if (entity == null) return NotFound();

            return await TransformToDto(entity);
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
            var isRestored = await _crud.RestoreEntity(id);

            return isRestored ? Ok() : NotFound(new { errorMessage = "Check entered id" });
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
        public async Task<ActionResult<List<TDto>>> GetAllDeleted()
        {
            var items = await _crud.GetAllDeletedEntities();
            List<TDto> itemsDto = new List<TDto>();

            foreach (var item in items)
            {
                itemsDto.Add(await TransformToDto(item));
            }
            return Ok(itemsDto);
        }

    }
}

