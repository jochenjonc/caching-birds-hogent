using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace BirdsApi.Data;

public class Observation
{
    protected Observation() {} // For EF Core

    public Observation(Bird bird)
    {
        Bird = bird;
        ObservationDate = DateTime.Now;
    }

    public int Id { get; private set; }

    public Bird Bird { get; set; }

    public DateTimeOffset ObservationDate { get; set; }

    public string? Remark { get; set; }

    public Point? Location { get; set; }

    [JsonIgnore]
    public Photo? Photo { get; set; }

    public bool HasPhoto => Photo != null;
}