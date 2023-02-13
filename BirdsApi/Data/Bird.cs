namespace BirdsApi.Data;

public class Bird
{
    protected Bird() { } // For EF Core

    public Bird(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }
}