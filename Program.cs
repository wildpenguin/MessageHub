using Google.Cloud.Firestore;
using MessageHub.Models;
using MessageHub.Repositories;
using MessageHub.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<FirestoreDb>(sp => {
    return FirestoreDb.Create("messagehub-aea2b");
});

builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IClientsService, ClientsService>();

builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<IEventsService, EventsService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
