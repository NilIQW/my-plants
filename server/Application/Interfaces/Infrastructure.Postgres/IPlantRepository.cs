using Core.Domain.Entities;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IPlantRepository
{
    Task<List<Plant>> GetAllAsync();
    Task<Plant?> GetByIdAsync(string id);
    Task AddAsync(Plant plant);
    Task AddUserPlantAsync(UserPlant userPlant); 

    void Update(Plant plant);
    void Delete(Plant plant);
    Task SaveChangesAsync();
}