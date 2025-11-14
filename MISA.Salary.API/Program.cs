using MISA.Salary.API;
using MISA.Salary.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDependencyLayer(config);

builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();

var app = builder.Build();

app.UseExceptionHandler((_) => { });

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
