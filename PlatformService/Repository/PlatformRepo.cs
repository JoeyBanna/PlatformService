using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PlatformService.Repository
{
    public class PlatformRepo : IPlatformRepo
    {
        public readonly ApplicationDbContext _Context;

        public PlatformRepo(ApplicationDbContext context)
        {
            _Context = context;
        }

        public void CreatePlatformAsync(Platform platform)
        {
             _Context.AddAsync(platform);
             _Context.SaveChanges();
        }

        public Platform Get(int id)
        {
            return _Context.Platforms.FirstOrDefault(item=> item.Id == id);
             
        }

        public IEnumerable<Platform> GetAllAsync()
        {
            return   _Context.Platforms.ToList();
           
        }

        public bool SaveChanges()
        {
            return (_Context.SaveChanges() >= 0);
        }
    }
}
