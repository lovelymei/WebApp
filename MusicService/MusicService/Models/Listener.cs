using EntitiesLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public partial class Listener : AccountBase
    {
        public Listener()
        {
            //Performers = new List<Performer>();
            Albums = new List<Album>();
            Songs = new List<Song>();
            IsDeleted = false;
        }

        /// <summary>
        /// Идентификатор слушателя
        /// </summary>
        public Guid ListenerId { get; set; }

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
