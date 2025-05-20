using Application.Interfaces.Infrastructure.Postgres;
using Core.Domain.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Postgres.Postgresql.Data;

public class WateringLogRepository : IWateringLogRepository
{
    private readonly MyDbContext _dbContext;

    public WateringLogRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WateringLog> CreateAsync(WateringLog log)
    {
        _dbContext.WateringLogs.Add(log);
        await _dbContext.SaveChangesAsync();
        return log;
    }

    public async Task<IEnumerable<WateringLog>> GetByPlantIdAsync(string plantId)
    {
        return await _dbContext.WateringLogs
            .Where(w => w.PlantId == plantId)
            .Include(w => w.TriggeredByUser)
            .Include(w => w.Plant)
            .OrderByDescending(w => w.Timestamp)
            .ToListAsync();
    }
}