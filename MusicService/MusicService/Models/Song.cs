using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public class Song
    {

        /// <summary>
        /// Идентификатор песни
        /// </summary>
        public Guid SongId { get; set; }

        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Возможный идентификатор Альбома 
        /// </summary>
        public Guid? AlbumId { get; set; }

        /// <summary>
        /// Альбом
        /// </summary>
        public Album Album { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Длительность
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// Дата выпуска
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Не удалена ли песня
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
        public Guid PerformerId { get; set; }

        /// <summary>
        /// Слушатели
        /// </summary>
        public List<Listener> Listeners { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Performer Performer { get; set; }


        //public virtual Listener Listener { get; set; }

        public Song()
        {
            //Listeners = new List<Listener>();
            IsDeleted = false;
        }

    }
}
