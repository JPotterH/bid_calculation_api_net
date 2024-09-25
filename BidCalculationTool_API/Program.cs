using Microsoft.EntityFrameworkCore;
using BidCalculationTool_API.Context;
using BidCalculationTool_API.Repositories.Interfaces;
using BidCalculationTool_API.Repositories;
using BidCalculationTool_API.Services.Interfaces;
using BidCalculationTool_API.Services;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the HTTP logging service on the builder instance
builder.Services.AddHttpLogging(opts =>
    opts.LoggingFields = HttpLoggingFields.RequestProperties);
builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Contexts - https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
builder.Services.AddDbContext<InMemoryContext>(opt => opt.UseInMemoryDatabase("VehicleBidding"));

// Repository operations reference
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();

// Services reference
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleFeeService, VehicleFeeService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();

//services cors
builder.Services.AddCors(p => p.AddPolicy("dev_cors",
    builder =>
    {
        // FE origins coul pass to external configuration instead of hardcoded at this point
        builder.WithOrigins("http://localhost:5173", "https://localhost:5173").AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
    }));


var app = builder.Build();

app.UseHttpLogging();

using (var serviceScope = app.Services.CreateScope())
{
    // DI for the data context initialization
    var scopedServices = serviceScope.ServiceProvider;

    var _vehicleRepository = scopedServices.GetRequiredService<IVehicleRepository>();
    _vehicleRepository?.InitInMemoryDb();

    var _vehicleTypeRepository = scopedServices.GetService<IVehicleTypeRepository>();
    _vehicleTypeRepository?.InitInMemoryDb();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("dev_cors");

    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
