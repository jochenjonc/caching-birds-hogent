using BirdsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BirdsApi.Services;

public class ObservationService : IObservationService
{
    private readonly BirdsDbContext _dbContext;

    public ObservationService(BirdsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Observation?> Find(int id)
    {
        return await _dbContext.Observations.FindAsync(id);
    }

    public async Task<List<Observation>> GetAll()
    {
        return await _dbContext.Observations.ToListAsync();
    }

    public async Task Add(Observation observation)
    {
        await _dbContext.Observations.AddAsync(observation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Observation observation)
    {
        _dbContext.Observations.Update(observation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(Observation observation)
    {
        _dbContext.Observations.Remove(observation);
        await _dbContext.SaveChangesAsync();
    }
}