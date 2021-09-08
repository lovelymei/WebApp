using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests.TestsServices
{
    class TestsRepositoryService
    {
        public static MusicDatabase GetClearDataBase()
        {
            var options = new DbContextOptionsBuilder<MusicDatabase>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MusicDatabase(options);
        }
    }
}
