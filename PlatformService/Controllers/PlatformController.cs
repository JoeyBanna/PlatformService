using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController :ControllerBase
    {
        public readonly IPlatformRepo _PlatformRepo;
        public readonly IMapper _mapper;


        public PlatformController(IPlatformRepo platformRepo, IMapper mapper)
        {
            _PlatformRepo = platformRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
          
            var obj = _PlatformRepo.GetAllAsync();
            if (obj == null)
            { 
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<Models.DTOs.PlatformDTO>>(obj));
              
        }
        [HttpPost("CreatePlatform")]
        public ActionResult<Models.DTOs.PlatformDTO> Create(Models.DTOs.PlatformCreateDTO platformCreateDTO)
        {
            
            var obj = _mapper.Map<Models.Platform>(platformCreateDTO);
             _PlatformRepo.CreatePlatformAsync(obj);
            _PlatformRepo.SaveChanges();
            var platform = _mapper.Map<Models.DTOs.PlatformDTO>(obj);

            return CreatedAtRoute(nameof(GetById), new {id = platform.Id},platform );
        }

        [HttpGet("{id}",Name = "GetById")]
        public ActionResult<Models.DTOs.PlatformDTO> GetById(int id)
        {
            var obj = _PlatformRepo.Get(id);
            if (obj != null)
            {
                var rest = _mapper.Map<Models.DTOs.PlatformDTO>(obj);
                return Ok(rest);

            }
            return NotFound();
           
        }

    }
}
