using Hangfire.Services.Services;
using HangFire.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangFireServices(builder.Configuration.GetConnectionString("HangfireConnection")?? String.Empty);

// Register our logging service for use with Dependency Injection
builder.Services.AddTransient<ILoggingService, LoggingService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

// Enable the Hangfire Dashboard -  VERY IMPORTANT
//  Make sure you can access it at /hangfire
app.RegisterHangfireDashboardMiddleware("/hangfire");

app.MapControllers();

app.Run();
