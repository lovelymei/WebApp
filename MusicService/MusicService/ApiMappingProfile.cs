using AutoMapper;
using MusicService.Dto;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            AllowNullCollections = true;
            CreateMap<Song, SongDto>();
        }
    }
}
