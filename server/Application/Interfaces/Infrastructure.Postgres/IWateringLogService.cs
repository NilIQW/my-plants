using Application.Models.Dtos.RestDtos.WateringLogDtos;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IWateringLogService
{
    Task<WateringLogDto> CreateAsync(CreateWateringLogDto dto);
    Task<IEnumerable<WateringLogDto>> GetByPlantIdAsync(string plantId);

}