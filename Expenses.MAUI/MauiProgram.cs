using CommunityToolkit.Maui;
using Expenses.MAUI.Extensions;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Expenses.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .UseAppSettings()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
                    .AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold")
                    .AddFont("Rubik-Light.ttf", "RubikLight")
                    .AddFont("Rubik-Regular.ttf", "RubikRegular")
                    .AddFont("Rubik-SemiBold.ttf", "RubikSemiBold")
                    .AddFont("Rubik-Bold.ttf", "RubikBold")
                    .AddFont("fa-solid-900.ttf", "FaSolid"); ;
            })
            .ConfigureServices();

        return builder.Build();
    }
}
