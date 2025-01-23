using Backend.FamilyTree;
using Backend.FamilyTree.Repositories;
using Backend.FamilyTree.Services;
using Backend.FamilyTree.SignalRNotifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.FamilyTree.Models;
using System.Security.Claims;
using System.Text;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using AuthenticationService = Backend.FamilyTree.Services.AuthenticationService;
using IAuthenticationService = Backend.FamilyTree.Services.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FamilyTreeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//// Configure ElasticSearch
//var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration["ElasticSearch:Uri"]))
//    .DefaultIndex("familytree");
//var client = new ElasticsearchClient(settings);
//builder.Services.AddSingleton(client);

// Configure ElasticSearch
var elasticUri = builder.Configuration["ElasticSearch:Uri"];
if (string.IsNullOrEmpty(elasticUri))
{
    throw new ArgumentNullException("ElasticSearch:Uri", "ElasticSearch URI is not configured.");
}

var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
    .DefaultIndex("familytree");
var client = new ElasticsearchClient(settings);
builder.Services.AddSingleton(client);

builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(typeof(Program)); // If AutoMapper is used

// Enable Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Family Tree",
        Version = "v1"
    });
});

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "yourissuer",
        ValidAudience = "youraudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yoursecretkey"))
    };

    // Configure SignalR to use the access token from the query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? throw new ArgumentNullException("Facebook AppId is not configured.");
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? throw new ArgumentNullException("Facebook AppSecret is not configured.");
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new ArgumentNullException("Google ClientId is not configured.");
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new ArgumentNullException("Google ClientSecret is not configured.");
})
.AddOAuth("Yahoo", options =>
{
    options.ClientId = builder.Configuration["Authentication:Yahoo:ClientId"] ?? throw new ArgumentNullException("Yahoo ClientId is not configured.");
    options.ClientSecret = builder.Configuration["Authentication:Yahoo:ClientSecret"] ?? throw new ArgumentNullException("Yahoo ClientSecret is not configured.");
    options.CallbackPath = new PathString("/signin-yahoo");
    options.AuthorizationEndpoint = "https://api.login.yahoo.com/oauth2/request_auth";
    options.TokenEndpoint = "https://api.login.yahoo.com/oauth2/get_token";
    options.UserInformationEndpoint = "https://api.login.yahoo.com/openid/v1/userinfo";
    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    options.SaveTokens = true;
});

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();