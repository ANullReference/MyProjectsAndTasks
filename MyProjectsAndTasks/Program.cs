using Asp.Versioning;
using Core;
using Core.Abstractions;
using Infrastructure.DatabaseRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MyProjectsAndTasks.DevEnvHelpers;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=host.docker.internal;Database=ProjectsAndTasks;Persist Security Info=True;User ID=sa;Password=Pointblank0;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Test\";Command Timeout=0"));


// Configure API versioning services
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ReadAccess", policy => policy.RequireRole("User", "Admin"))
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // 3. Map Scalar to /scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Projects and Tasks")
               .WithTheme(ScalarTheme.DeepSpace); // Optional theme
    });

    //test authentication while not being able to use identity provider. 
    //For production we would use JWT tokens or something similar.
    builder.Services.AddAuthentication("DevScheme")
        .AddScheme<AuthenticationSchemeOptions, DevelopmentAuthenticationHandler>("DevScheme", null);
}

if (app.Environment.IsProduction() && builder.Services.Any(s => s.ServiceType == typeof(DevelopmentAuthenticationHandler)))
{
    throw new InvalidOperationException("SECURITY ALERT: DevAuthHandler detected in Production configuration!");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
