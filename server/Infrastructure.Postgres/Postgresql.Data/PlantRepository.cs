using Application.Interfaces.Infrastructure.Postgres;
using Core.Domain.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Postgres.Postgresql.Data;

public class PlantRepository : IPlantRepository
{
    private readonly MyDbContext _db;

    public PlantRepository(MyDbContext db)
    {
        _db = db;
    }

    public async Task<List<Plant>> GetAllAsync()
    {
        return await _db.Plants.ToListAsync();
    }

    public async Task<Plant?> GetByIdAsync(string id)
    {
        return await _db.Plants.FindAsync(id);
    }

    public async Task AddAsync(Plant plant)
    {
        await _db.Plants.AddAsync(plant);
    }

    public async Task AddUserPlantAsync(UserPlant userPlant)
    {
        await _db.UserPlants.AddAsync(userPlant);
    }

    public void Update(Plant plant)
    {
        _db.Plants.Update(plant);
    }

    public void Delete(Plant plant)
    {
        _db.Plants.Remove(plant);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}