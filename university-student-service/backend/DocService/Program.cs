using Microsoft.EntityFrameworkCore;
using DocService.Data.DbContexts;
using DocService.Services;
using DocService.Data;
using Amazon.S3;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Load secrets from AWS
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

// Configure AWS S3 client with credentials from appsettings.json
var awsConfig = builder.Configuration.GetSection("AWS").Get<AWSConfig>();
var credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
var s3Config = new AmazonS3Config
{
    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsConfig.Region)
};
builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Configuration.AddEnvironmentVariables();

// Add CORS policy for local Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "http://127.0.0.1:4200",
                "http://localhost:4200",
                "http://3.107.49.76",
                "http://university-alb-1516057962.us-east-1.elb.amazonaws.com"
                )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure AWS RDS connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Custom configuration class for AWS settings
public class AWSConfig
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region { get; set; }
}
