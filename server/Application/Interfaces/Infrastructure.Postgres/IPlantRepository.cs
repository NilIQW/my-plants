using Core.Domain.Entities;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IPlantRepository
{
    Task<List<Plant>> GetAllAsync();
    Task<Plant?> GetByIdAsync(Guid id);
    Task AddAsync(Plant plant);
    void Update(Plant plant);
    void Delete(Plant plant);
    Task SaveChangesAsync();
}