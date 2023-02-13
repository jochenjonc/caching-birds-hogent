using BirdsApi.Data;

namespace BirdsApi.Services;

public interface IObservationService
{
    ValueTask<Observation?> Find(int id);
    Task<List<Observation>> GetAll();
    Task Add(Observation observation);
    Task Update(Observation observation);
    Task Remove(Observation observation);
}