using MusicService.Entity;
using System;
using System.Collections.Generic;

namespace MusicService.Models
{
    public class Album : EntityBase
    {

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Идентификатор пользователя (внешний ключ)
        /// </summary>
        public Guid? PerformerId { get; set; }

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

        public Album() : base()
        {
            Listeners = new List<Listener>();
            Songs = new List<Song>();
        }
    }
}
