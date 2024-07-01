using AzureBatchEndpoint;
using AzureBatchEndpoint.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<MLService>();
builder.Services.AddScoped<AzureMLBatchClient>();
builder.Services.AddScoped<AzureMLBatchClientOptions>();
builder.Services.AddScoped<AzureStorageAccountClient>();
builder.Services.AddScoped<AzureStorageAccountOptions>();

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

app.MapControllers();

app.Run();
