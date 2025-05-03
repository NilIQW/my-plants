using Application.Models.Dtos.RestDtos.PlantDtos;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IPlantService
{
    Task<List<PlantDto>> GetAllAsync();
    Task<PlantDto?> GetByIdAsync(string id);
    Task<PlantDto> CreateAsync(CreatePlantDto dto);
    Task<PlantDto?> UpdateAsync(string id, UpdatePlantDto dto);
    Task<bool> DeleteAsync(string id);
}