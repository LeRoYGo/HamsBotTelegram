using System.Text.Json.Serialization;

namespace HamsBotTelegram
{
    public class Coin
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("current_price")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("market_cap")]
        public int MarketCap { get; set; }

        [JsonPropertyName("total_volume")]
        public double TotalVolume { get; set; }

        [JsonPropertyName("high_24h")]
        public double High24h { get; set; }

        [JsonPropertyName("low_24h")]
        public double Low24h { get; set; }

        [JsonPropertyName("price_change_percentage_1h_in_currency")]
        public double PriceChangePercentage1hInCurrency { get; set; }

        [JsonPropertyName("price_change_percentage_24h_in_currency")]
        public double PriceChangePercentage24hInCurrency { get; set; }

        [JsonPropertyName("price_change_percentage_7d_in_currency")]
        public double PriceChangePercentage7dInCurrency { get; set; }
    }
}
