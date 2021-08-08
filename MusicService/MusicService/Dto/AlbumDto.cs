using MusicService.Models;
using System;

namespace MusicService.Dto
{
    public class AlbumDto : AccountBaseDto
    {
        public AlbumDto(Album album)
        {
            Name = album.Name;
            ProductionDate = album.ProductionDate;
        }
        public string Name { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
