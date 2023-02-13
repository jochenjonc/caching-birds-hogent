using BirdsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BirdsApi.Services;

public class BirdService : IBirdService
{
    private readonly BirdsDbContext _dbContext;

    public BirdService(BirdsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<Bird?> Find(int id)
    {
        return await _dbContext.Birds.FindAsync(id);
    }

    public async Task<List<Bird>> GetAll()
    {
        return await _dbContext.Birds.ToListAsync();
    }
}