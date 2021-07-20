
using EntitiesLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public partial class Performer : AccountBase
    {
        public Performer()
        {
            //  Listeners = new List<Listener>();
            Songs = new List<Song>();
            IsDeleted = false;
        }


        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Песни
        /// </summary>
        public virtual List<Song> Songs { get; set; }

        // public virtual List<Listener> Listeners { get; set; }

        /// <summary>
        /// Альбомы
        /// </summary>
        public List<Album> Albums { get; set; }
    }
}
