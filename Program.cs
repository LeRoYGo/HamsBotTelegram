using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HamsBotTelegram
{
    class Program
    {
        private static ITelegramBotClient _bot = new TelegramBotClient("Token");

        static void Main()
        {
            Console.WriteLine("Bot launched " + _bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions();
            _bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
            Console.ReadLine();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message?.Text != null)
            {
                switch (update.Message.Text.ToLower())
                {
                    case { } message when message.Contains("/price"):
                        await PreparingMessagePriceTeam(update);
                        break;
                    case { } message when message.Contains("/farm"):
                        await PreparingMessageFarmTeam(update);
                        break;
                    case { } message when message.Contains("buy"):
                        await PreparingMessageBuyTeam(update);
                        break;
                    case { } message when message.Contains("nft"):
                        await PreparingMessageNftTeam(update);
                        break;
                }
            }

        }

        static async Task PreparingMessagePriceTeam(Update update)
        {
            Coin coin = new Coin().GetApiDataAsync().Result;
            string coinInfo = $"{coin.name} - ${coin.symbol.ToUpper()}\n" +
                              $"💰 Price: ${coin.current_price ?? 0:f5}\n" +
                              $"⚖️ H: ${coin.high_24h ?? 0:f5}" +
                              $" | L: ${coin.low_24h ?? 0:f5}\n" +
                              $"🌚 1h: {coin.price_change_percentage_1h_in_currency ?? 0:f2}% \n" +
                              $"🌚 24h: {coin.price_change_percentage_24h_in_currency ?? 0:f2}% \n" +
                              $"📈 7d: {coin.price_change_percentage_7d_in_currency ?? 0:f2}% \n" +
                              $"📊 Volume: ${coin.total_volume ?? 0:f2}\n";
            await SendMessage(update.Message?.Chat, coinInfo);
        }

        static async Task PreparingMessageBuyTeam(Update update)
        {
            var listMarketPrice = new InlineKeyboardMarkup(new[]{
                new [] {InlineKeyboardButton.WithUrl(text: "HAMS DEX", url: "https://dex.solhamster.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "DexLab", url: "https://trade.dexlab.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "Aldrin", url: "https://dex.aldrin.com/chart/spot/HAMS_USDC"), InlineKeyboardButton.WithUrl(text: "LoverDEX", url: "https://samoyedlovers.co/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "NoGoalDex", url: "https://dex.nogoal.click/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "Cato Dex", url: "https://catodex.com/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "Raydium Swap", url: "https://raydium.io/swap/?ammId=z2KxiSejQmNNsyxLFHbrewNLDeGLFZahFNSLYht2FFs"), InlineKeyboardButton.WithUrl(text: "Jupiter Swap", url: "https://jup.ag/swap/HAMS-USDC")}});
            await SendMessage(update.Message?.Chat, "Buy $HAMS. Click 👇", listMarketPrice);
        }

        static async Task PreparingMessageNftTeam(Update update)
        {
            var listMarketPriceNft = new InlineKeyboardMarkup(new[] {
                new [] {InlineKeyboardButton.WithUrl(text: "Metaplex", url: "https://hams.holaplex.com/#/"), InlineKeyboardButton.WithUrl(text: "DigitalEyes", url: "https://digitaleyes.market/collections/Space%20Hamster")}});
            await SendMessage(update.Message?.Chat, "Buy $HAMS NFT collection", listMarketPriceNft);
        }

        private static async Task PreparingMessageFarmTeam(Update update)
        {
            var listMarketPriceFarm = InlineKeyboardButton.WithUrl(text: "Cropper Farm", url: "https://cropper.finance/farms/?s=Be5mLMaSg1PpBJbK3P6DMAsd9arGi6xeDEApwEVeyHau");
            await SendMessage(update.Message?.Chat, "Farm $HAMS. Click 👇", listMarketPriceFarm);
        }

        private static async Task SendMessage(Chat chatId, string message, InlineKeyboardMarkup interactionButtons = null)
        {
            await _bot.SendTextMessageAsync(chatId, message, replyMarkup: interactionButtons);
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(exception));
            return Task.CompletedTask;
        }
    }
}