using BirdsApi.Data;

namespace BirdsApi.Services;

public interface IBirdService
{
    ValueTask<Bird?> Find(int id);

    Task<List<Bird>> GetAll();
}