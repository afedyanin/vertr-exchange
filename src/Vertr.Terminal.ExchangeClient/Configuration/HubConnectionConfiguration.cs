namespace Vertr.Terminal.ExchangeClient.Configuration;
public class HubConnectionConfiguration
{
    public static readonly string SectionName = "ExchangeHub";

    public string BaseUrl { get; set; } = "http://localhost:5000/exchange";
}
