using TwitchAnalytics.Exceptions;
using TwitchAnalytics.Infrastructure.InversionOfControls;
using TwitchAnalytics.Intrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddInfrastructureServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder.Services.Configure<TwitchAPIConfiguration>(app.Configuration.GetSection("ConnectionStrings.TwitchAPI"));

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
