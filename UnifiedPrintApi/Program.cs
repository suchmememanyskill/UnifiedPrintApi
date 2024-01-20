using UnifiedPrintApi.Service;
using UnifiedPrintApi.Service.MakerWorld;
using UnifiedPrintApi.Service.MMF;
using UnifiedPrintApi.Service.Printables;
using UnifiedPrintApi.Service.Thingiverse;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Cache>();
builder.Services.AddSingleton<ThingiverseApi>();
builder.Services.AddSingleton<MMFApi>();
builder.Services.AddSingleton<PrintablesApi>();
builder.Services.AddSingleton<MakerWorldApi>();
builder.Services.AddSingleton<Apis>();
builder.Services.AddSingleton<Storage>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");