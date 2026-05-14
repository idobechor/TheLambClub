using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TheLambClub.Models;
using TheLambClub.Services;

namespace TheLambClub
{
    public static class MauiProgram
    {
        #region public methods

        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<IPokerSuggestionService>(_ => new PokerSuggestionService(Keys.OpenAIApiKey));
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

        #endregion
    }
}
