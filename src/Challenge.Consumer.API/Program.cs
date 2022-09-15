using Challenge.Consumer.API;
using Challenge.Consumer.API.Collectors;
using Challenge.Consumer.API.Events;
using Challenge.Consumer.API.Metrics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//DI
builder.Services.AddMetrics();
builder.Services.AddHandlers();
builder.Services.AddDispatchers();
builder.Services.AddTwitterStreams(builder.Configuration);

builder.Services.AddSingleton<IHashtagCollector, HashtagCollector>();
builder.Services.AddSingleton<ITwitterMetrics, TwitterMetricsService>();

var app = builder.Build();
app.StreamEvents()
    .Stream<TweetReceived>();

app.MapGet("/", () => "Welcome to Twitter Streaming Client");
app.MapGet("/metrics", ([FromServices] ITwitterMetrics metricService) => {
    var dto = new {
        data = metricService.GetMetricsData(),
        top10Hashtags = metricService.GetTopTenHashtags()
    };

    return Results.Ok(dto);
});

app.Run();
