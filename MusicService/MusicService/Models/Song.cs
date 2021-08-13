﻿using NewEntityLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace MusicService.Models
{
    public class Song : EntityBase
    {

        /// <summary>
        /// Идентификатор аккаунта (внешний ключ)
        /// </summary>
        public Guid FK_AccountId { get; set; }

        /// <summary>
        /// Возможный идентификатор Альбома (внешний ключ)
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

        public Song()
        {
            IsDeleted = false;
        }

    }
}
