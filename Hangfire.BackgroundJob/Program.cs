using Hangfire;
using Hangfire.BackgroundJob.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Hangfire
// Database has to be created beforehand
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"))); // Use SQL Server

// Add the processing server as a hosted service.  This starts Hangfire
// server within your ASP.NET Core application.  This is what *processes*
// the background jobs.
builder.Services.AddHangfireServer();

// Register our logging service for use with Dependency Injection
builder.Services.AddTransient<ILoggingService, LoggingService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

// Enable the Hangfire Dashboard -  VERY IMPORTANT
app.UseHangfireDashboard("/hangfire"); //  Make sure you can access it at /hangfire

app.MapControllers();

app.Run();
