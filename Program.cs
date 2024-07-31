using AzureBatchEndpoint;
using AzureBatchEndpoint.Clients;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Diamond Praisal API",
        Description = "This API allows for diamond price praisal. ",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context => Task.Run(() => 
        context.Response.Redirect($"/swagger/index.html")));

app.Run();
