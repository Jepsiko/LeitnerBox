using LeitnerBox.ViewModel;
using Microsoft.Extensions.Logging;

namespace LeitnerBox
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<SubjectPage>();
            builder.Services.AddTransient<SubjectViewModel>();

            builder.Services.AddTransient<QuestionPage>();
            builder.Services.AddTransient<QuestionViewModel>();

            builder.Services.AddTransient<DrawerPage>();
            builder.Services.AddTransient<DrawerViewModel>();

            builder.Services.AddTransient<TrainPage>();
            builder.Services.AddTransient<TrainViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}