using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HamsBotTelegram
{
    class Coin
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("current_price")]
        public object CurrentPrice { get; set; }
        [JsonProperty("total_volume")]
        public object TotalVolume { get; set; }
        [JsonProperty("high_24h")]
        public object High24h { get; set; }
        [JsonProperty("low_24h")]
        public object Low24h { get; set; }
        [JsonProperty("price_change_percentage_1h_in_currency")]
        public object PriceChangePercentage1hCurrency { get; set; }
        [JsonProperty("price_change_percentage_24h_in_currency")]
        public object PriceChangePercentage24hCurrency { get; set; }
        [JsonProperty("price_change_percentage_7d_in_currency")]
        public object PriceChangePercentage7dCurrency { get; set; }

        public async Task<Coin> GetApiDataAsync()
        {
            using var client = new HttpClient();
            var task = await client.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=space-hamster&order=market_cap_desc&per_page=100&page=1&sparkline=false&price_change_percentage=1h%2C24h%2C7d");
            var jsonString = await task.Content.ReadAsStringAsync();
            var coins = JsonConvert.DeserializeObject<Coin[]>(jsonString);
            return coins?.Single();
        }
    }
}
