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
    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
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

builder.Services.AddDbContext<ExpensesContext>(dbBuilder =>
{
    var configuration = builder.Configuration;
    dbBuilder.UseSqlite(configuration.GetConnectionString("Default"))
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        .EnableDetailedErrors();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Authentication:Authority"];
    options.Audience = builder.Configuration["Authentication:Audience"];
});

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ExpensesContext>();
    context.Database.Migrate();
}
catch (Exception e)
{
    var logger = app.Services.GetRequiredService<ILogger<ExpensesContext>>();
    logger.LogError(e, "Exception occurred while migrating database: {Error}", e.Message);
}

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
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
