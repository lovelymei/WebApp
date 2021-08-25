using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Entity
{
    public abstract class EntityBaseDto
    {
        public EntityBaseDto(EntityBase entityBase)
        {
            IsDeleted = entityBase.IsDeleted;
        }

        public bool IsDeleted { get; set; }
    }
}
