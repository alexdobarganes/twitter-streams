namespace Challenge.Consumer.API.Infrastructure;

public class TwitterApiOptions
{
    public const string SectionName = "Twitter";
    public string StreamBaseUrl { get; set; } = string.Empty;
    public string BearerToken { get; set; } = string.Empty;
}