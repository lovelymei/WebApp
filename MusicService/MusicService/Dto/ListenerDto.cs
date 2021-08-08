using AspNetCoreValidationLibrary;
using MusicService.Dto;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public class ListenerDto : AccountBaseDto
    {
        public ListenerDto(Listener user) : base(user)
        {
            BirthDate = user.BirthDate;
        }

        public DateTime BirthDate { get; set; }
    }
}

