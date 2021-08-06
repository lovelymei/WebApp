using EntitiesLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public class Listener : AccountBase
    {
        public Listener()
        {
            Albums = new List<Album>();
            Songs = new List<Song>();
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Добавленные песни
        /// </summary>
        public List<Song> Songs { get; set; }

        /// <summary>
        /// Добавленные альбомы
        /// </summary>
        public List<Album> Albums { get; set; }
    }
}
