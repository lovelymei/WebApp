using MusicService.Models;
using NewEntityLibrary;
using System;

namespace MusicService.Dto
{
    public class AlbumDto : EntityBaseDto
    {
        public AlbumDto(Album entity) : base(entity)
        {
            Name = entity.Name;
            ProductionDate = entity.ProductionDate;
        }
        public string Name { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
