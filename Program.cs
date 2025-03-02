using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using InmindLab3_4part2.Data;
using InmindLab3_4part2.Services;
using AutoMapper;
using InmindLab3_4part2.Profiles;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add API Explorer + Swagger (NO custom file upload filter)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "University API",
        Version = "v1"
    });
    // Removed: c.OperationFilter<CustomFileUploadOperationFilter>();
});

// Add database context
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add Blob Storage Service
string blobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage") ?? "UseDevelopmentStorage=true";
try
{
    builder.Services.AddSingleton<IBlobStorageService>(provider =>
        new BlobStorageService(blobConnectionString));
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing BlobStorageService: {ex.Message}");
}

// Add Keycloak Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "http://localhost:8080/auth/realms/university";
    options.Audience = "university-api";
    options.RequireHttpsMetadata = false; // For dev only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsStudent", policy => policy.RequireRole("student"));
    options.AddPolicy("IsTeacher", policy => policy.RequireRole("teacher"));
    options.AddPolicy("IsAdmin", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure the university-assets directory exists
var universityAssetsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "university-assets");
if (!Directory.Exists(universityAssetsPath))
{
    Directory.CreateDirectory(universityAssetsPath);
}

// Serve static files
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(universityAssetsPath),
    RequestPath = "/university-assets"
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
