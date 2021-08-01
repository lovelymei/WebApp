using EntitiesLibrary;
using Microsoft.AspNetCore.Mvc;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    public abstract class CrudControllerBase<T>:ControllerBase where T: AccountBase
    {
        protected readonly ICrud<T> _crud;

        protected CrudControllerBase(ICrud<T> crud)
        {
            _crud = crud;
        }


    }
}
