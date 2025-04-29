using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.PlantDtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantsController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantsController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantResponseDto>>> GetAll()
        {
            var plants = await _plantService.GetAllAsync();

            // Convert to PlantResponseDto
            var response = plants.Select(plant => new PlantResponseDto
            {
                Id = plant.Id,
                PlantName = plant.PlantName,
                PlantType = plant.PlantType,
                MoistureLevel = plant.MoistureLevel, // Example attribute
                IsAutoWateringEnabled = plant.IsAutoWateringEnabled
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PlantResponseDto>> GetById(Guid id)
        {
            var plant = await _plantService.GetByIdAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            // Map to PlantResponseDto
            var response = new PlantResponseDto
            {
                Id = plant.Id,
                PlantName = plant.PlantName,
                PlantType = plant.PlantType,
                MoistureLevel = plant.MoistureLevel, // Example attribute
                IsAutoWateringEnabled = plant.IsAutoWateringEnabled
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlantDto dto)
        {
            var created = await _plantService.CreateAsync(dto);

            var response = new PlantResponseDto
            {
                Id = created.Id,
                PlantName = created.PlantName,
                PlantType = created.PlantType,
                MoistureLevel = created.MoistureLevel, 
                IsAutoWateringEnabled = created.IsAutoWateringEnabled
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdatePlantDto dto)
        {
            var updated = await _plantService.UpdateAsync(id, dto);

            if (updated == null)
            {
                return NotFound();
            }

            var response = new PlantResponseDto
            {
                Id = updated.Id,
                PlantName = updated.PlantName,
                PlantType = updated.PlantType,
                MoistureLevel = updated.MoistureLevel, // Example attribute
                IsAutoWateringEnabled = updated.IsAutoWateringEnabled
            };

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _plantService.DeleteAsync(id);

            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
