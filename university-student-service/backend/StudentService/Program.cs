using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using StudentService;
var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<StudentService.Data.IStudentRepository, StudentService.Data.StudentRepository>();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<StudentService.Data.StudentRepository>();


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

app.Run();
