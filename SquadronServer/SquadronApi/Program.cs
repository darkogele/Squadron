using Microsoft.EntityFrameworkCore;
using SquadronApi.Data;
using SquadronApi.Middleware;
using SquadronApi.Registers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .RegisterCors()
    .RegisterSwagger()
    .ApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDoc();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCORS();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();

    // Cheeks if data base exists if not it will create one and apply migrations
    await context.Database.MigrateAsync();

    // If we need to seed Initial Data bellow 

}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

await app.RunAsync();
