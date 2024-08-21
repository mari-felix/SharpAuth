using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using API.Context;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<UserContext>();

builder.Services.AddTransient<UserServices>();
builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyHeader() // Permitir quaisquer cabe�alhos personalizados
            .AllowAnyMethod() // Permitir qualquer m�todo HTTP (GET, POST, PUT, DELETE, etc.)
            .AllowAnyOrigin());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();