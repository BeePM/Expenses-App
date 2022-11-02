using System.Net;
using System.Reflection;
using Expenses.MAUI.Interfaces;
using Expenses.MAUI.Services;
using Expenses.MAUI.Views;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;

namespace Expenses.MAUI.Extensions;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseAppSettings(this MauiAppBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.json");
        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        builder.Configuration.AddConfiguration(config);
        return builder;
    }

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddHttpClient("Api")
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(m => m.StatusCode == HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(5, ra => TimeSpan.FromSeconds(Math.Pow(2, ra))));

        builder.Services.AddSingleton<App>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardView>();

        builder.Services.AddSingleton<IExpenseService, ExpenseService>();

        return builder;
    }
}