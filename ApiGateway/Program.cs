using Application.Models;
using Domain.Repositories;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories.Base;
using Infrastructure.Repositories.Base;
using Domain.Services.Interfaces;
using Domain.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SmartChargingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Group, GroupDTO>();
    cfg.CreateMap<ChargeStation, ChargeStationDTO>();
    cfg.CreateMap<Connector, ConnectorDTO>();

    cfg.AllowNullCollections = true;
    cfg.AllowNullDestinationValues = true;
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IChargeStationService, ChargeStationService>();
builder.Services.AddScoped<IConnectorService, ConnectorService>();
builder.Services.AddScoped<ISmartChargeDomainService, SmartChargeDomainService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();
builder.Services.AddScoped<IConnectorRepository, ConnectorRepository>();



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
