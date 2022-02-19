using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegram.Bot
{
    class Program
    {
        private static TelegramBotClient botClient;
        private const string urlCoinString = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=space-hamster&order=market_cap_desc&per_page=100&page=1&sparkline=false&price_change_percentage=1h%2C24h%2C7d";


        static void Main()
        {
            botClient = new TelegramBotClient("2016040925:AAGxwLKV6ZikKCqVHkrT4fQ27c9zQ8ry-QU");
            botClient.OnMessage += OnMessageHandlerAsync;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void OnMessageHandlerAsync(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                switch (e.Message.Text.ToLower())
                {
                    case string message when message.Contains("/price"):
                        await PreparingMessagePriceTeam(e);
                        break;
                    case string message when message.Contains("/farm"):
                        await PreparingMessageFarmTeam(e);
                        break;
                    case string message when message.Contains("buy"):
                        await PreparingMessageBuyTeam(e);
                        break;
                    case string message when message.Contains("nft"):
                        await PreparingMessageNftTeam(e);
                        break;
                }
            }

            Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id} an user name {e.Message.Chat.Username}.");
        }

        static async Task PreparingMessageBuyTeam(MessageEventArgs e)
        {
            var listMarketPrice = new InlineKeyboardMarkup(new[]{
                  new [] {InlineKeyboardButton.WithUrl(text: "HAMS DEX", url: "https://dex.solhamster.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "DexLab", url: "https://trade.dexlab.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                  new [] {InlineKeyboardButton.WithUrl(text: "Aldrin", url: "https://dex.aldrin.com/chart/spot/HAMS_USDC"), InlineKeyboardButton.WithUrl(text: "LoverDEX", url: "https://samoyedlovers.co/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                  new [] {InlineKeyboardButton.WithUrl(text: "NoGoalDex", url: "https://dex.nogoal.click/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "Cato Dex", url: "https://catodex.com/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                  new [] {InlineKeyboardButton.WithUrl(text: "Raydium Swap", url: "https://raydium.io/swap/?ammId=z2KxiSejQmNNsyxLFHbrewNLDeGLFZahFNSLYht2FFs"), InlineKeyboardButton.WithUrl(text: "Jupiter Swap", url: "https://jup.ag/swap/HAMS-USDC")}});
            await SendButtonMessage(e.Message.Chat, "Buy $HAMS. Click 👇", listMarketPrice);
        }

        static async Task PreparingMessageFarmTeam(MessageEventArgs e)
        {
            var listMarketPriceFARM = InlineKeyboardButton.WithUrl(text: "Cropper Farm", url: "https://cropper.finance/farms/?s=Be5mLMaSg1PpBJbK3P6DMAsd9arGi6xeDEApwEVeyHau");
            await SendButtonMessage(e.Message.Chat, "Farm $HAMS. Click 👇", listMarketPriceFARM);
        }

        static async Task PreparingMessageNftTeam(MessageEventArgs e)
        {
            var listMarketPriceNFT = new InlineKeyboardMarkup(new[] {
                    new [] {InlineKeyboardButton.WithUrl(text: "Metaplex", url: "https://hams.holaplex.com/#/"), InlineKeyboardButton.WithUrl(text: "DigitalEyes", url: "https://digitaleyes.market/collections/Space%20Hamster")}});
            await SendButtonMessage(e.Message.Chat, "Buy $HAMS NFT collection", listMarketPriceNFT);
        }

        static async Task PreparingMessagePriceTeam(MessageEventArgs e)
        {
            var client = new HttpClient();
            var task = await client.GetAsync(urlCoinString);
            var jsonString = await task.Content.ReadAsStringAsync();
            Coin[] coin = JsonConvert.DeserializeObject<Coin[]>(jsonString);
            string coinInfo = $"{coin[0].name} - ${coin[0].symbol.ToUpper()}\n" +
                              $"💰 Price: ${string.Format("{0:f5}", coin[0].current_price ?? 0)}\n" +
                              $"⚖️ H: ${string.Format("{0:f5}", coin[0].high_24h ?? 0)}" +
                              $" | L: ${string.Format("{0:f5}", coin[0].low_24h ?? 0)}\n" +
                              $"🌚 1h: {string.Format("{0:f2}", coin[0].price_change_percentage_1h_in_currency ?? 0)}% \n" +
                              $"🌚 24h: {string.Format("{0:f2}", coin[0].price_change_percentage_24h_in_currency ?? 0)}% \n" +
                              $"📈 7d: {string.Format("{0:f2}", coin[0].price_change_percentage_7d_in_currency ?? 0)}% \n" +
                              $"📊 Volume: ${string.Format("{0:f2}", coin[0].total_volume ?? 0)}\n";
            await SendMessage(e.Message.Chat, coinInfo);
        }

        static async Task SendMessage(Chat chatId, string message)
        {
            await botClient.SendTextMessageAsync(
             chatId: chatId,
             text: message
           );
        }

        static async Task SendButtonMessage(Chat chatId, string message, InlineKeyboardMarkup interactionButtons)
        {
            await botClient.SendTextMessageAsync(
             chatId: chatId,
             text: message,
             replyMarkup: interactionButtons
           );
        }
    }

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
    }
}