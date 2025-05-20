using Application.Interfaces.Infrastructure.MQTT;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.WateringLogDtos;
using Core.Domain.Entities;

namespace Application.Services;

public class WateringLogService : IWateringLogService
{
    private readonly IWateringLogRepository _repository;

    public WateringLogService(IWateringLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<WateringLogDto> CreateAsync(CreateWateringLogDto dto)
    {
        var log = new WateringLog
        {
            PlantId = dto.PlantId,
            TriggeredByUserId = dto.TriggeredByUserId,
            Timestamp = DateTime.UtcNow,
            Method = dto.Method
        };

        var created = await _repository.CreateAsync(log);

        return new WateringLogDto
        {
            Id = created.Id,
            PlantId = created.PlantId,
            TriggeredByUserId = created.TriggeredByUserId,
            Timestamp = created.Timestamp,
            Method = created.Method
        };
    }
    
    public async Task<IEnumerable<WateringLogDto>> GetByPlantIdAsync(string plantId)
    {
        var logs = await _repository.GetByPlantIdAsync(plantId);

        return logs.Select(log => new WateringLogDto
        {
            Id = log.Id,
            PlantId = log.PlantId,
            TriggeredByUserId = log.TriggeredByUserId,
            Timestamp = log.Timestamp,
            Method = log.Method
        });
    }

}

