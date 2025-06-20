using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using EnrolmentService.Data;
using EnrolmentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins(
            "http://127.0.0.1:4200",
            "http://localhost:4200",
            "http://3.107.49.76",
            "http://18.98.196.74",
            "http://18.98.196.73",
            "http://university-alb-425533074.ap-southeast-2.elb.amazonaws.com"
            )
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Load secrets from AWS
var secretName = "Prod/AuthService/Jwt";
var region = Amazon.RegionEndpoint.APSoutheast2;

var secretsClient = new AmazonSecretsManagerClient(region);
var secretResponse = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
{
    SecretId = secretName
});

if (!string.IsNullOrEmpty(secretResponse.SecretString))
{
    var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(secretResponse.SecretString);
    builder.Configuration.AddInMemoryCollection(secrets!);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");
string issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing from configuration.");
string audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing from configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddScoped<IStudentEnrolmentService, StudentEnrolmentService>();

// Configure Kestrel and URL binding
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5001);
});
builder.WebHost.UseUrls("http://0.0.0.0:5001");

var app = builder.Build();
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();