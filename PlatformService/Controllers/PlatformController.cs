using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Models.DTOs;
using PlatformService.Repository;
using PlatformService.SyncDataServices.HTTP;
using System;
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
        public readonly ICommandDataClient _commandDataClient;
        public readonly IMessageBusClient _messageBusClient;


        public PlatformController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient,IMessageBusClient messageBusClient)
        {
            _PlatformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;

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
        public async Task< ActionResult<Models.DTOs.PlatformDTO>> Create(Models.DTOs.PlatformCreateDTO platformCreateDTO)
        {
            
            var obj = _mapper.Map<Models.Platform>(platformCreateDTO);
             _PlatformRepo.CreatePlatformAsync(obj);
            _PlatformRepo.SaveChanges();
            var platform = _mapper.Map<Models.DTOs.PlatformDTO>(obj);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platform);
            }catch(Exception ex)
            {
              Console.WriteLine($"----> could not send to command platform synchronously{ex.Message}");

            }
            try
            {
                var platformPublishDto= _mapper.Map<PlatformPublishedDto>(platform);
                platformPublishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishDto);

            }catch(Exception ex)
            {
              Console.WriteLine($"----> could not send to asynchronously platform{ex.Message}");

            }

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
