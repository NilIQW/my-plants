using Application.Interfaces.Infrastructure.Postgres;
using Application.Models.Dtos.RestDtos.PlantDtos;
using Core.Domain.Entities;

namespace Application.Services;

public class PlantService : IPlantService
    {
        private readonly IPlantRepository _repository;

        public PlantService(IPlantRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PlantDto>> GetAllAsync()
        {
            var plants = await _repository.GetAllAsync();
            return plants.Select(MapToDto).ToList();
        }

        public async Task<PlantDto?> GetByIdAsync(Guid id)
        {
            var plant = await _repository.GetByIdAsync(id);
            return plant == null ? null : MapToDto(plant);
        }

        public async Task<PlantDto> CreateAsync(CreatePlantDto dto)
        {
            var plant = new Plant
            {
                PlantName = dto.PlantName,
                PlantType = dto.PlantType,
                MoistureThreshold = dto.MoistureThreshold,
                MoistureLevel = dto.MoistureLevel,
                IsAutoWateringEnabled = dto.IsAutoWateringEnabled,
            };

            await _repository.AddAsync(plant);
            await _repository.SaveChangesAsync();

            return MapToDto(plant);
        }

        public async Task<PlantDto?> UpdateAsync(Guid id, UpdatePlantDto dto)
        {
            var plant = await _repository.GetByIdAsync(id);
            if (plant == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.PlantName))
                plant.PlantName = dto.PlantName;

            if (!string.IsNullOrWhiteSpace(dto.PlantType))
                plant.PlantType = dto.PlantType;

            if (dto.MoistureLevel.HasValue)
                plant.MoistureLevel = dto.MoistureLevel.Value;

            if (dto.MoistureThreshold.HasValue)
                plant.MoistureThreshold = dto.MoistureThreshold.Value;

            if (dto.IsAutoWateringEnabled.HasValue)
                plant.IsAutoWateringEnabled = dto.IsAutoWateringEnabled.Value;

            _repository.Update(plant);
            await _repository.SaveChangesAsync();

            return MapToDto(plant);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var plant = await _repository.GetByIdAsync(id);
            if (plant == null) return false;

            _repository.Delete(plant);
            await _repository.SaveChangesAsync();
            return true;
        }

        private static PlantDto MapToDto(Plant plant) => new PlantDto
        {
            Id = plant.Id,
            PlantName = plant.PlantName,
            PlantType = plant.PlantType,
            MoistureLevel = plant.MoistureLevel,
            MoistureThreshold = plant.MoistureThreshold,
            IsAutoWateringEnabled = plant.IsAutoWateringEnabled
        };
    }


