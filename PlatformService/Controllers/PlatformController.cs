using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using platformservice.SyncDataServices.Http;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.models;

namespace PlatformService.Controllers
{
    [Route("api/platforms")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        public PlatformController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _commandDataClient = commandDataClient;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("---> Getting platforms");

            var platformItem = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatrformById")]
        public ActionResult<PlatformReadDTO> GetPlatrformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);

            if (platformItem != null)
                return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            else
                return NotFound();
        }

        [HttpPost("create")]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform([FromBody] PlatformCreateDTO platform)
        {
            var platformModel = _mapper.Map<Platform>(platform);
            _repository.CreatePlatfrom(platformModel);
            _repository.SaveChanges();

            var platformReadModel = _mapper.Map<PlatformReadDTO>(platformModel);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatrformById), new { Id = platformReadModel.Id }, platformReadModel);
        }
    }
}