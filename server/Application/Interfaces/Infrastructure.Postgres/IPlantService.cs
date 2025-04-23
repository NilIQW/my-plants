using Application.Models.Dtos.RestDtos.PlantDtos;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IPlantService
{
    Task<List<PlantDto>> GetAllAsync();
    Task<PlantDto?> GetByIdAsync(Guid id);
    Task<PlantDto> CreateAsync(CreatePlantDto dto);
    Task<PlantDto?> UpdateAsync(Guid id, UpdatePlantDto dto);
    Task<bool> DeleteAsync(Guid id);
}