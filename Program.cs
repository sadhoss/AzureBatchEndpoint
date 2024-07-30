using AzureBatchEndpoint;
using AzureBatchEndpoint.Clients;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context => Task.Run(() => 
        context.Response.Redirect($"/swagger/index.html")));

app.Run();
