using System;
using System.Collections.Generic;

namespace MusicService.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string Name { get; set; }
        public DateTime ProductionDate { get; set; }
        public bool IsDeleted { get; set; }

        public Guid AccountId { get; set; }
        public Performer Performer { get; set; }
        public List<Song> Songs { get; set; }
        public List<Listener> Listeners { get; set; }

        public Album()
        {
            Listeners = new List<Listener>();
            Songs = new List<Song>();
        }

    }
}
