using AspNetCoreValidationLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public class ListenerDto
    {
        public ListenerDto(Listener user)
        {
            BirthDate = user.BirthDate;
        }

        public DateTime BirthDate { get; set; }
        public virtual List<PerformerDto> Performers { get; set; }
    }
}

