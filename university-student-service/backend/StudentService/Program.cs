using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentService;
using System.Text;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "StudentService", Version = "v1" });

    // Add JWT support
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "http://127.0.0.1:4200",
                "http://localhost:4200",
                "http://3.107.49.76",
                "http://18.98.196.73",
                "http://18.98.196.74",
                "http://university-alb-425533074.ap-southeast-2.elb.amazonaws.com/"
                )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
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
//
builder.Services.AddScoped<StudentService.Data.IStudentRepository, StudentService.Data.StudentRepository>();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<StudentService.Data.StudentRepository>();

//nant
string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");
string issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing from configuration.");
string audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing from configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.Events = new JwtBearerEvents
        //{
        //    OnMessageReceived = context =>
        //    {
        //        var token = context.Request.Cookies["AuthToken"];
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            context.Token = token;
        //        }

        //        return Task.CompletedTask;
        //    }
        //};
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
//

// Configure Kestrel and URL binding
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5003);
});
builder.WebHost.UseUrls("http://0.0.0.0:5003");

var app = builder.Build();

// ✅ HTTPS first
app.UseHttpsRedirection();

// ✅ Then CORS
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ Swagger last (only for development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var creds = FallbackCredentialsFactory.GetCredentials();
Console.WriteLine("✅ AWS Access Key in use: " + creds.GetCredentials().AccessKey);

app.Run();
