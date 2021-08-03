using EntitiesLibrary;
using System;
using System.Collections.Generic;

namespace MusicService.Models
{
    public class Album : AccountBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid AlbumId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Удален ли альбом
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public Performer Performer { get; set; }

        /// <summary>
        /// Песни в альбоме
        /// </summary>
        public List<Song> Songs { get; set; }

        /// <summary>
        /// Слушатели
        /// </summary>
        public List<Listener> Listeners { get; set; }

        public Album()
        {
            Listeners = new List<Listener>();
            Songs = new List<Song>();
        }

    }
}
