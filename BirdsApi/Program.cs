using BirdsApi;
using BirdsApi.Data;
using BirdsApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

NetTopologySuite.NtsGeometryServices.Instance = new NetTopologySuite.NtsGeometryServices(
    // default CoordinateSequenceFactory
    NetTopologySuite.Geometries.Implementation.CoordinateArraySequenceFactory.Instance,
    // default precision model
    new NetTopologySuite.Geometries.PrecisionModel(100000d),
    // default SRID
    4326,
    // Geometry overlay operation function set to use (Legacy or NG)
    NetTopologySuite.Geometries.GeometryOverlay.NG,
    // Coordinate equality comparer to use (CoordinateEqualityComparer or PerOrdinateEqualityComparer)
    new NetTopologySuite.Geometries.CoordinateEqualityComparer());


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBirdService, BirdService>();
builder.Services.AddTransient<IObservationService, ObservationService>();

builder.Services.AddDbContext<BirdsDbContext>((serviceProvider, configure) => configure
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.UseNetTopologySuite())
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapGroup("/api/birds")
    .MapBirdsApi()
    .WithTags("Bird Endpoints");

app.MapGroup("/api/observations")
    .MapObservationsApi()
    .WithTags("Observation Endpoints");

app.Run();