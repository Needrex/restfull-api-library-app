using Pomelo.EntityFrameworkCore.MySql;
using RestApiApp.Data;
using Microsoft.EntityFrameworkCore;
using RestApiApp.Repositories;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Middlewares;
using RestApiApp.Configurations;
using RestApiApp.Services;
using RestApiApp.InterfaceServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using RestApiApp.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

builder.Services.Configure<FrontendOptions>(builder.Configuration.GetSection("Frontend"));
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);



var apiKeySettings = builder.Configuration.GetSection("Api").Get<ApiKeySettings>();
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection("Api"));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // untuk dev
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };


    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst("UserId")?.Value;

            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            var refreshToken = await db.RefreshToken.FirstOrDefaultAsync(refreshToken => refreshToken.UserId == Convert.ToInt32(userId));

            if (refreshToken == null || refreshToken.IsRevoked)
            {
                context.Fail("Unauthorized: user not active or revoked");
            }
        },

        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("VerifiedOnly", policy =>
        policy.RequireClaim("IsVerified", "True"));

// Register DI (Dependecy Injection)
builder.Services.AddAuthorization();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserOtpsRepository, UserOtpsRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();


builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddSingleton<IRazorViewRenderer, RazorViewRenderer>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBooksServices, BooksService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AuthProfile));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Database.CanConnect())
    {
        Console.WriteLine("Tidak bisa terhubung ke database. Periksa koneksi atau konfigurasi.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
// app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
