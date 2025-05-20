using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.WateringLogDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WateringLogController : ControllerBase
{
    private readonly IWateringLogService _wateringLogService;

    public WateringLogController(IWateringLogService wateringLogService)
    {
        _wateringLogService = wateringLogService;
    }

    [HttpPost]
    public async Task<ActionResult<WateringLogDto>> Create([FromBody] CreateWateringLogDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _wateringLogService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetByPlantId), new { plantId = dto.PlantId }, result);
    }


    [HttpGet("plant/{plantId}")]
    public async Task<ActionResult<List<WateringLogDto>>> GetByPlantId(string plantId)
    {
        var wateringLogs = await _wateringLogService.GetByPlantIdAsync(plantId);

        var response = wateringLogs.Select(log => new WateringLogDto
        {
            Id = log.Id,
            PlantId = log.PlantId,
            TriggeredByUserId = log.TriggeredByUserId,
            Timestamp = log.Timestamp,
            Method = log.Method
        }).ToList();

        return Ok(response);
    }

}