using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HamsBotTelegram
{
    class Coin
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public object current_price { get; set; }
        public object total_volume { get; set; }
        public object high_24h { get; set; }
        public object low_24h { get; set; }
        public object price_change_percentage_1h_in_currency { get; set; }
        public object price_change_percentage_24h_in_currency { get; set; }
        public object price_change_percentage_7d_in_currency { get; set; }

        public async Task<Coin> GetApiDataAsync()
        {
            var client = new HttpClient();
            var task = await client.GetAsync("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=space-hamster&order=market_cap_desc&per_page=100&page=1&sparkline=false&price_change_percentage=1h%2C24h%2C7d");
            var jsonString = await task.Content.ReadAsStringAsync();
            var coins = JsonConvert.DeserializeObject<Coin[]>(jsonString);
            return coins?.Single();
        }
    }
}
