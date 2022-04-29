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
        private static ITelegramBotClient _bot = new TelegramBotClient("TOKEN");

        static void Main()
        {
            Console.WriteLine("Bot launched " + _bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions();
            _bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
            Console.ReadLine();
        }

        public static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message?.Text != null)
            {
                switch (update.Message.Text.ToLower())
                {
                    case string message when message.Contains("/price"):
                        PreparingMessagePriceTeam(update);
                        break;
                    case string message when message.Contains("/farm"):
                        PreparingMessageFarmTeam(update);
                        break;
                    case string message when message.Contains("buy"):
                        PreparingMessageBuyTeam(update);
                        break;
                    case string message when message.Contains("nft"):
                        PreparingMessageNftTeam(update);
                        break;
                }
            }
            return Task.CompletedTask;
        }

        static void PreparingMessagePriceTeam(Update update)
        {
            Coin coin = new Coin().GetApiDataAsync().Result;
            string coinInfo = $"{coin.Name} - ${coin.Symbol.ToUpper()}\n" +
                              $"💰 Price: ${coin.CurrentPrice ?? 0:f5}\n" +
                              $"⚖️ H: ${coin.High24h ?? 0:f5}" +
                              $" | L: ${coin.Low24h ?? 0:f5}\n" +
                              $"🌚 1h: {coin.PriceChangePercentage1hCurrency ?? 0:f2}% \n" +
                              $"🌚 24h: {coin.PriceChangePercentage24hCurrency ?? 0:f2}% \n" +
                              $"📈 7d: {coin.PriceChangePercentage7dCurrency ?? 0:f2}% \n" +
                              $"📊 Volume: ${coin.TotalVolume ?? 0:f2}\n";
            SendMessage(update.Message?.Chat, coinInfo);
        }

        static void PreparingMessageBuyTeam(Update update)
        {
            var listMarketPrice = new InlineKeyboardMarkup(new[]{
                new [] {InlineKeyboardButton.WithUrl(text: "HAMS DEX", url: "https://dex.solhamster.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "DexLab", url: "https://trade.dexlab.space/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "Aldrin", url: "https://dex.aldrin.com/chart/spot/HAMS_USDC"), InlineKeyboardButton.WithUrl(text: "LoverDEX", url: "https://samoyedlovers.co/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "NoGoalDex", url: "https://dex.nogoal.click/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv"), InlineKeyboardButton.WithUrl(text: "Cato Dex", url: "https://catodex.com/#/market/5j6hdwx4eW3QBYZtRjKiUj7aDA1dxDpveSHBznwq7kUv")},
                new [] {InlineKeyboardButton.WithUrl(text: "Raydium Swap", url: "https://raydium.io/swap/?ammId=z2KxiSejQmNNsyxLFHbrewNLDeGLFZahFNSLYht2FFs"), InlineKeyboardButton.WithUrl(text: "Jupiter Swap", url: "https://jup.ag/swap/HAMS-USDC")}});
            SendMessage(update.Message?.Chat, "Buy $HAMS. Click 👇", listMarketPrice);
        }

        static void PreparingMessageNftTeam(Update update)
        {
            var listMarketPriceNft = new InlineKeyboardMarkup(new[] {
                new [] {InlineKeyboardButton.WithUrl(text: "Metaplex", url: "https://hams.holaplex.com/#/"), InlineKeyboardButton.WithUrl(text: "DigitalEyes", url: "https://digitaleyes.market/collections/Space%20Hamster")}});
            SendMessage(update.Message?.Chat, "Buy $HAMS NFT collection", listMarketPriceNft);
        }

        private static void PreparingMessageFarmTeam(Update update)
        {
            var listMarketPriceFarm = InlineKeyboardButton.WithUrl(text: "Cropper Farm", url: "https://cropper.finance/farms/?s=Cf9tsFKWLPVegdibYQfHvbxwg9LoR4Go2UnuJ9gQh5ga");
            SendMessage(update.Message?.Chat, "Farm $HAMS. Click 👇", listMarketPriceFarm);
        }

        private static void SendMessage(Chat chatId, string message, InlineKeyboardMarkup interactionButtons = null)
        {
            _bot.SendTextMessageAsync(chatId, message, replyMarkup: interactionButtons);
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(exception));
            return Task.CompletedTask;
        }
    }
}