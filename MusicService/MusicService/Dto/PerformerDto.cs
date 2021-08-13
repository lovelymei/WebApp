using AspNetCoreValidationLibrary;
using NewEntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace MusicService.Models
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public partial class PerformerDto : EntityBaseDto
    {
        public PerformerDto(Performer user) : base(user)
        {
            BirthDate = user.BirthDate;
            Songs = user.Songs
                .Select(c => new SongDto(c))
                .ToList();
        }

        [Required]
        public DateTime BirthDate { get; set; }
        public virtual List<SongDto> Songs { get; set; }
    }
}
