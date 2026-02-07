using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TheLambClub.Models;
using TheLambClub.Services;

namespace TheLambClub
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            string? apiKey = Keys.OpenAIKey;
            builder.Services.AddSingleton<IPokerSuggestionService>(_ => new PokerSuggestionService(apiKey));
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
