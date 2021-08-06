
using EntitiesLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public class Performer : AccountBase
    {
        public Performer()
        {
            Songs = new List<Song>();
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Песни
        /// </summary>
        public virtual List<Song> Songs { get; set; }

        /// <summary>
        /// Альбомы
        /// </summary>
        public List<Album> Albums { get; set; }
    }
}
