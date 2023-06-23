using PlatformService.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Repository
{
    public interface IPlatformRepo
    {
         bool SaveChanges();
        IEnumerable<Models.Platform> GetAllAsync();
        Models.Platform Get(int id);
        void CreatePlatformAsync(Models.Platform platform);

    }
}
