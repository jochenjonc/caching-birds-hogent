using BirdsApi.Data;
using BirdsApi.Services;
using CsvHelper;
using FluentValidation;
using NetTopologySuite.IO;
using System.Globalization;
using Point = NetTopologySuite.Geometries.Point;
using SkiaSharp;

namespace BirdsApi;

public static class ObservationEndpoint
{
    public static RouteGroupBuilder MapObservationsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllObservations);
        group.MapGet("/{id}", GetObservation);

        group.MapPost("/", CreateObservation)
            .AddEndpointFilter<ValidationFilter<ObservationRequest>>();

        group.MapPut("/{id}", UpdateObservation)
            .AddEndpointFilter<ValidationFilter<ObservationRequest>>();

        group.MapDelete("/{id}", DeleteObservation);


        group.MapGet("/{id}/photo", GetObservationPhoto);

        group.MapPut("/{id}/photo", UploadObservationPhoto);

        group.MapDelete("/{id}/photo", DeleteObservationPhoto);


        group.MapPost("/upload", UploadObservations);

        return group;
    }

    // get all observations
    public static async Task<IResult> GetAllObservations(IObservationService observationService)
    {
        var observations = await observationService.GetAll();
        return TypedResults.Ok(observations);
    }

    // get observation by id
    public static async Task<IResult> GetObservation(int id, IObservationService observationService)
    {
        var observation = await observationService.Find(id);

        if (observation != null)
        {
            return TypedResults.Ok(observation);
        }

        return TypedResults.NotFound();
    }

    // create observation
    public static async Task<IResult> CreateObservation(ObservationRequest observation, IObservationService observationService, IBirdService birdService)
    {
        var bird = await birdService.Find(observation.BirdId!.Value);

        if (bird == null)
        {
            return TypedResults.BadRequest($"Bird with id {observation.BirdId} does not exist.");
        }

        var newObservation = new Observation(bird)
        {
            ObservationDate = observation.ObservationDate!.Value,
            Location = observation.Location,
            Remark = observation.Remark,
        };

        await observationService.Add(newObservation);

        return TypedResults.Created($"/observations/{newObservation.Id}", newObservation);
    }

    // update observation
    public static async Task<IResult> UpdateObservation(int id, ObservationRequest observation, IObservationService observationService, IBirdService birdService)
    {
        var existingObservation = await observationService.Find(id);

        if (existingObservation != null)
        {
            var bird = await birdService.Find(observation.BirdId!.Value);

            if (bird == null)
            {
                return TypedResults.BadRequest($"Bird with id {observation.BirdId} does not exist.");
            }

            existingObservation.Bird = bird;
            existingObservation.ObservationDate = observation.ObservationDate!.Value;
            existingObservation.Location = observation.Location;
            existingObservation.Remark = observation.Remark;

            await observationService.Update(existingObservation);

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    // delete observation
    public static async Task<IResult> DeleteObservation(int id, IObservationService observationService)
    {
        var observation = await observationService.Find(id);

        if (observation != null)
        {
            await observationService.Remove(observation);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }


    // get observation photo
    public static async Task<IResult> GetObservationPhoto(int id, IObservationService observationService, int? width, int? height)
    {
        var observation = await observationService.Find(id);

        if (observation != null && observation.Photo != null)
        {
            var photo = observation.Photo;

            if (width == null && height == null)
            {
                return Results.File(photo.Data, photo.ContentType, photo.Name);
            }
            else
            {
                var bitmap = SKBitmap.Decode(photo.Data);

                var ratioWidth = width != null ? (float)width / bitmap.Width : 0;
                var ratioHeight = height != null ? (float)height / bitmap.Height : 0;

                var scale = Math.Max(ratioWidth, ratioHeight);

                var newWidth = (int)(bitmap.Width * scale);
                var newHeight = (int)(bitmap.Height * scale);

                using var surface = SKSurface.Create(new SKImageInfo(newWidth, newHeight));
                using var canvas = surface.Canvas;

                var sourceRect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                var destRect = new SKRect(0, 0, newWidth, newHeight);

                canvas.DrawBitmap(bitmap, sourceRect, destRect);

                var newBitmap = SKBitmap.FromImage(surface.Snapshot());

                var image = SKImage.FromBitmap(newBitmap);
                var data = image.Encode(SKEncodedImageFormat.Webp, 90);

                return Results.File(data.ToArray(), "image/webp", Path.ChangeExtension(photo.Name, "webp"));
            }
        }

        return TypedResults.NotFound();
    }

    // upload observation photo
    public static async Task<IResult> UploadObservationPhoto(int id, IFormFile file, IObservationService observationService)
    {
        var observation = await observationService.Find(id);

        if (observation != null)
        {
            using var photoStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(photoStream);

            observation.Photo = new Photo(file.FileName, file.ContentType, photoStream.ToArray());

            await observationService.Update(observation);

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    // delete observation photo
    public static async Task<IResult> DeleteObservationPhoto(int id, IObservationService observationService)
    {
        var observation = await observationService.Find(id);

        if (observation != null)
        {
            observation.Photo = null;

            await observationService.Update(observation);

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }


    // upload multiple observations
    public static async Task<IResult> UploadObservations(IFormFile file, IObservationService observationService, IBirdService birdService)
    {
        using var reader = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8);
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var anonymousObservationDefinition = new
            {
                BirdId = default(int),
                Location = string.Empty,
                Remark = string.Empty
            };

            var observations = csv.GetRecords(anonymousObservationDefinition);
            foreach (var observation in observations)
            {
                var bird = await birdService.Find(observation.BirdId);

                if (bird == null)
                {
                    return TypedResults.BadRequest($"Bird with id {observation.BirdId} does not exist.");
                }

                var newObservation = new Observation(bird)
                {
                    Location = string.IsNullOrWhiteSpace(observation.Location) ? null : new WKTReader().Read(observation.Location) as Point,
                    Remark = string.IsNullOrWhiteSpace(observation.Remark) ? null : observation.Remark
                    // Photo = NOT SUPPORTED
                };

                await observationService.Add(newObservation);
            }
        }

        return TypedResults.NoContent();
    }
}

public record ObservationRequest(int? BirdId, DateTimeOffset? ObservationDate, Point? Location, string? Remark, byte[]? Photo)
{
    public class Validator : AbstractValidator<ObservationRequest>
    {
        public Validator()
        {
            RuleFor(x => x.BirdId).NotNull();
            RuleFor(x => x.ObservationDate).NotNull();
        }
    }
}