using BookstoreAPI.Application.UseCases.Auth;
using BookstoreAPI.Application.UseCases.Books;
using BookstoreAPI.Application.UseCases.Orders;
using BookstoreAPI.Application.UseCases.Users;
using BookstoreAPI.Application.UseCases.Genres;
using BookstoreAPI.Domain.Repositories;
using BookstoreAPI.Domain.Services;
using BookstoreAPI.Infrastructure.Persistence;
using BookstoreAPI.Infrastructure.Persistence.Repositories;
using BookstoreAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use PORT environment variable (required for Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    // Global rate limiting policy - 100 requests per minute per IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    // Strict policy for authentication endpoints - 5 attempts per 15 minutes per IP
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(15),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    // Standard policy for API endpoints - 30 requests per minute per IP
    options.AddPolicy("api", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Database
// Parse Render's DATABASE_URL or use ConnectionStrings__DefaultConnection
var connectionString = GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseNpgsql(connectionString));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
        });
});

// Add JWT Authentication
// Try to get JWT secret from environment variable first, then from configuration
var secretKey = Environment.GetEnvironmentVariable("JwtSettings__SecretKey")
                ?? builder.Configuration.GetSection("JwtSettings")["SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Register Repositories (Infrastructure)
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Register Domain Services (Infrastructure implementations)
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Register Use Cases (Application)
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<GetAllBooksUseCase>();
builder.Services.AddScoped<GetBookByIdUseCase>();
builder.Services.AddScoped<CreateBookUseCase>();
builder.Services.AddScoped<UpdateBookUseCase>();
builder.Services.AddScoped<DeleteBookUseCase>();
builder.Services.AddScoped<GetAllOrdersUseCase>();
builder.Services.AddScoped<GetOrdersByUserIdUseCase>();
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<CancelOrderUseCase>();
builder.Services.AddScoped<CompleteOrderUseCase>();
builder.Services.AddScoped<CloseOrderUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GetAllGenresUseCase>();
builder.Services.AddScoped<GetGenreByIdUseCase>();
builder.Services.AddScoped<CreateGenreUseCase>();
builder.Services.AddScoped<UpdateGenreUseCase>();
builder.Services.AddScoped<DeleteGenreUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline
// Enable Swagger in all environments for testing
app.UseSwagger();
app.UseSwaggerUI();

// Only use HTTPS redirection in production with valid certificates
if (!app.Environment.IsDevelopment())
{
    // Render handles HTTPS at the load balancer level
    app.UseForwardedHeaders();
}
else
{
    app.UseHttpsRedirection();
}

// Enable rate limiting middleware (must be before UseAuthentication/UseAuthorization)
app.UseRateLimiter();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Helper method to parse database connection string
static string GetConnectionString(IConfiguration configuration)
{
    // First, try to get DATABASE_URL (Render's format)
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        try
        {
            // Parse the DATABASE_URL into Npgsql connection string format
            var uri = new Uri(databaseUrl);
            var db = uri.AbsolutePath.Trim('/');

            // Split user info and decode properly
            var userInfo = uri.UserInfo.Split(':', 2); // Limit to 2 parts in case password contains ':'

            if (userInfo.Length != 2)
            {
                throw new InvalidOperationException("Invalid DATABASE_URL format: missing username or password");
            }

            var username = Uri.UnescapeDataString(userInfo[0]);
            var password = Uri.UnescapeDataString(userInfo[1]);

            // Build connection string with properly decoded credentials
            var connStr = $"Host={uri.Host};Port={uri.Port};Database={db};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

            return connStr;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to parse DATABASE_URL: {ex.Message}", ex);
        }
    }

    // Fallback to ConnectionStrings__DefaultConnection or appsettings.json
    return Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
           ?? configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}
