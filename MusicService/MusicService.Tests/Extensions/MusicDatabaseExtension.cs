using MusicService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests.Extensions
{
    public static class MusicDatabaseExtension 
    {
        public static void AddCollection(this MusicDatabase db, IEnumerable<EntityBase> collection)
        {
            foreach (var entityBase in collection)
            {
                db.Add(entityBase);
            }
            db.SaveChanges();
        }

        public static void AddEntity(this MusicDatabase db, EntityBase entity)
        {
            db.Add(entity);       
            db.SaveChanges();
        }
    }
}
