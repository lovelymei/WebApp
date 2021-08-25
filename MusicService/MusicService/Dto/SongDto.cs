using AspNetCoreValidationLibrary;
using MusicService.Entity;
using MusicService.Models;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MusicService.Dto
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public partial class SongDto : EntityBaseDto
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
