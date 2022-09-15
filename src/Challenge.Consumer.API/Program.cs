using Challenge.Consumer.API;
using Challenge.Consumer.API.Domain;
using Challenge.Consumer.API.DTO;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//DI
builder.Services.AddHandlers();
builder.Services.AddDispatchers();
builder.Services.AddTwitterStreams(builder.Configuration);

builder.Services.AddSingleton<IMetricService, MetricService>();

var app = builder.Build();

app.StreamEvents()
    .Stream<TweetReceived>();

app.MapGet("/", () => "Welcome to Twitter Streaming Client");
app.MapGet("/metrics", ([FromServices] IMetricService metricService) => {
    var dto = new {
        tweetsPerSecond = metricService.GetEntriesPerSecond(),
        totalTweetsReceived = metricService.GetTotalNumberOfEntriesReceived(),
        totalHashtagsFound = metricService.GetTotalNumberHashtagsFound(),
        top10Hashtags = metricService.GetTopTenHashtags()
    };

    return Results.Ok(dto);
});

app.Run();
