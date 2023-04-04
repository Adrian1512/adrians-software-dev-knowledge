using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    if (builder.Environment.IsDevelopment())
    {
        Console.WriteLine("--> Using in mem db");
        opt.UseInMemoryDatabase("InMem");
    }
    else
    {
        Console.WriteLine($"--> Using sql server${builder.Configuration.GetConnectionString("PlatformsConn")}");
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
    }
});

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();


Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapGrpcService<GrpcPlatformService>();
app.MapControllers();

app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
