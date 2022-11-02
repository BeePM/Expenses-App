using System.Text.Json;
using System.Text.Json.Serialization;
using Expenses.API.Database;
using Expenses.API.Filters;
using Expenses.API.Interfaces;
using Expenses.API.Middleware;
using Expenses.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(o => o.AddServerHeader = false);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(UserLoggingFilter));
    options.UseDateOnlyTimeOnlyStringConverters();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.UseDateOnlyTimeOnlyStringConverters();
});
;
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{builder.Environment.ApplicationName}.xml");
    c.UseDateOnlyTimeOnlyStringConverters();
});

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<UserLoggingFilter>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUserService>(sp =>
{
    if (builder.Environment.IsDevelopment())
    {
        return new DevelopmentUserService();
    }

    var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    return new UserService(contextAccessor);
});

var configuration = builder.Configuration;
builder.Services.AddDbContext<IExpensesContext, ExpensesContext>(dbBuilder =>
{
    if (builder.Environment.IsProduction())
    {
        dbBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
    }
    else
    {
        dbBuilder.UseInMemoryDatabase("localdb", mo => mo.EnableNullChecks());
    }

    dbBuilder.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        .EnableDetailedErrors();
});

if (builder.Configuration.GetValue<bool>("Authentication:IsEnabled"))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
    });
}

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (builder.Configuration.GetValue<bool>("Authentication:IsEnabled"))
{
    app.UseAuthentication();
}

app.UseAuthorization();

if (builder.Environment.IsDevelopment())
{
    app.MapControllers().AllowAnonymous();
}
else
{
    app.MapControllers();
}

app.Run();
