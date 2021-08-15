using AspNetCoreValidationLibrary;
using MusicService.Models;
using NewEntityLibrary;
using System;

#nullable disable

namespace MusicService.Dto
{
    [DateFormat]
    [OnlyLatin]
    [Length]
    public class ListenerDto : EntityBaseDto
    {
        public ListenerDto(Listener user) : base(user)
        {
            BirthDate = user.BirthDate;
        }

        public DateTime BirthDate { get; set; }
    }
}

