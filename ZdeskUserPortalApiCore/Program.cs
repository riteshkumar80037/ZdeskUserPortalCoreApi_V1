using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ZdeskUserPortal.Business;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Business.Services;
using ZdeskUserPortal.DataAccess;
using ZdeskUserPortal.DataAccess.EntityFramwork.Context;
using ZdeskUserPortal.DataAccess.Home;
using ZdeskUserPortal.DataAccess.RepositoryServices;
using ZdeskUserPortal.DataAccess.RepositoryServices.Home;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortalApiCore.Extension;
using ZdeskUserPortalApiCore.JWTToken;
using ZdeskUserPortalApiCore.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
builder.Services.AddControllers();

builder.Logging.ClearProviders();
builder.Host.UseNLog();
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
    .Build();

//Sql connection for Entity Framwork
builder.Services.AddDbContext<ZdeskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZdeskConnection")));

//Redis Cache Connection
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Replace with your Redis connection string
    options.InstanceName = "SampleInstance";  // Optional prefix for keys
});

//Auto Mapper 
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IConfiguration>(config);

// Learn more about configuring Swagger/OpenAPI at
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Security scheme for JWT
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Require token in all endpoints
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            securityScheme, new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var privateKey = builder.Configuration.GetValue<string>("PrivateKey");


builder.Services.Configure<AuthSettings>(builder.Configuration);
// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedApisPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//builder.Services.AddSingleton<AuthSettings>();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddTransient<LoginAccess>();
builder.Services.AddScoped(typeof(ILoginRepository), typeof(LoginRepositoryServices));
builder.Services.AddScoped<ILogin, LoginServices>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepositoryServices<>));
builder.Services.AddScoped<IBusinessUnit, BusinessUnitServices>();
builder.Services.AddScoped<IRefereshToken, RefereshTokenServices>();
builder.Services.AddScoped<IMaster, MasterServices>();
// Assign the configuration to the static property
SqlDbConnectionFactory.StaticConfiguration = builder.Configuration;

// Add versioning
builder.Services.UseAppVersioningHandler();

// Add model validation
builder.Services.UseModelValidationHandler();

// Add Token Validate
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime=true,
            ValidIssuer = "abc",
            ValidAudience = "abc",
            ClockSkew = TimeSpan.Zero

        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireRole("user"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");
if (enableSwagger)
{

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}
// Global Exception Middleware
app.UseGlobalExceptionHandler();
app.UseCors("AllowedApisPolicy");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
