using Azure.Storage.Blobs;
using System.ComponentModel;
using BusinessLayer.Implementation;
using BusinessLayer.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(u => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureConnection")));
builder.Services.AddScoped<IContainerService, ContainerService>();
builder.Services.AddScoped<IBlobService, BlobService>();

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("ReactCORS", options =>
    {
        options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("ReactCORS");

app.MapControllers();

app.Run();
