namespace Vertr.Terminal.ExchangeClient.Configuration;
public class ExchangeConfiguration
{
    public static readonly string SectionName = "Exchange";

    public string HubConnectionBaseUrl { get; set; } = "http://localhost:5000/exchange";
}
