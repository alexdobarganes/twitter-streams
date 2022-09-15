using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using Humanizer;

namespace Challenge.Consumer.API.Infrastructure;

public class TwitterStreamClient : ITwitterStreamClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<TwitterApiOptions> _options;
    private readonly ILogger<TwitterStreamClient> _logger;

    public TwitterStreamClient(IOptions<TwitterApiOptions> options, ILogger<TwitterStreamClient> logger)
    {
        _options = options;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_options.Value.StreamBaseUrl),
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Value.BearerToken);
        _options = options;
        _logger = logger;
    }

    private string GetTweetFields(Type type)
    {
        var properties = type.GetProperties().Select(x => x.Name.Underscore().ToLowerInvariant());
        return "tweet.fields=" + string.Join(',', properties);
    }

    public async Task StartStreamAsync<T>(Action<T> callback, CancellationToken cancellationToken = default)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));
        try
        {
            _logger.LogInformation("Connecting to the stream");

            using HttpResponseMessage response = await _httpClient.GetAsync($"?{GetTweetFields(typeof(T))}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Connected successfuly to the stream");

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var tweets = new StreamReader(stream);

            while (!cancellationToken.IsCancellationRequested)
            {
                var str = await tweets.ReadLineAsync();
                if (string.IsNullOrEmpty(str))
                    continue;

                using var document = JsonDocument.Parse(str);
                if (document.RootElement.TryGetProperty("data", out JsonElement data))
                {
                    _logger.LogDebug("New tweet received: {tweet}", str);
                    var tweet = data.Deserialize<T>(new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
                    });
                    if (tweet != null)
                        callback(tweet);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            // Conversion to other naming convention goes here. Like SnakeCase, KebabCase etc.
            return name.Underscore();
        }
    }
}
