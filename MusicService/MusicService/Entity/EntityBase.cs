﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Entity
{
    public class EntityBase
    {
        public Guid EntityId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
