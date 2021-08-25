using MusicService.Entity;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public class Listener : EntityBase
    {
        public Listener()
        {
            Albums = new List<Album>();
            Songs = new List<Song>();
        }

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
