using Core.Domain.Entities;

namespace Application.Interfaces.Infrastructure.Postgres;

public interface IWateringLogRepository
{
    Task<WateringLog> CreateAsync(WateringLog log);
    Task<IEnumerable<WateringLog>> GetByPlantIdAsync(string plantId);


}