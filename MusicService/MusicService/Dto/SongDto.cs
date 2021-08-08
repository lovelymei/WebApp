using AspNetCoreValidationLibrary;
using MusicService.Dto;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MusicService.Models
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public partial class SongDto : AccountBaseDto
    {
        public SongDto(Song entity) : base(entity)
        {
            Title = entity.Title;
            DurationMs = entity.DurationMs;
            ProductionDate = entity.ProductionDate;
        }

        [Required]
        public string Title { get; set; }
        public long DurationMs { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
