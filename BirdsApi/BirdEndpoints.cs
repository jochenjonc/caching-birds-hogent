using BirdsApi.Services;

namespace BirdsApi;

public static class BirdEndpoints
{
    public static RouteGroupBuilder MapBirdsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllBirds);

        return group;
    }

    // get all birds
    public static async Task<IResult> GetAllBirds(IBirdService birdService)
    {
        var birds = await birdService.GetAll();
        return TypedResults.Ok(birds);
    }
}