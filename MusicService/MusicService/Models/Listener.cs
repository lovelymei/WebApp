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
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Добавленные песни
        /// </summary>
        public List<Song> Songs { get; set; }

        public virtual List<Performer> Performers { get; set; }

        /// <summary>
        /// Добавленные альбомы
        /// </summary>
        public List<Album> Albums { get; set; }
    }
}
