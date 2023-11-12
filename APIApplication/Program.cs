using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using DataAccess;
using DataAccess.Objects;
using DemoAPIApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDbConnection>((_) => new SqlConnection(Configuration.GetConnectionString()));

builder.Services.AddScoped<IGrandParentsRepository, GrandParentsRepository>();
builder.Services.AddScoped<IParentsRepository, ParentsRepository>();
builder.Services.AddScoped<IChildrenRepository, ChildrenRepository>();

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddSingleton<IMapper>(mapperConfiguration.CreateMapper());

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

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

IMapper mapper = config.CreateMapper();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();