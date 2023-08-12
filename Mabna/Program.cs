//using Infrastructure;
using Application.ErrorHandler;
using Application.InstrumentLastTrade;
using Domain.Abstractions;
using Domain.CustomException;
using Infrastructure.DataBaseWithNoRelation;
using Infrastructure.DataBaseRelationBase;
using Mabna.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Mabna.WebAPI.SecondQuestion.Properties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalErrorHandler>();
builder.Services.AddTransient<IErrorHandler, ErrorHandlerService>();
builder.Services.AddScoped<IInstrumentsTradeService, InstrumentsLastTradeService>();

// There is a relation between tables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                    ?? throw new MyCustomException(Resource.Exception_ProgramCS_Empty_ConnectionString);
connectionString = String.Format(connectionString,
                                 Environment.GetEnvironmentVariable("DB_HOST"),
                                 Environment.GetEnvironmentVariable("DB_Name"),
                                 Environment.GetEnvironmentVariable("DB_Password"));

builder.Services.AddDbContext<RelationBaseDBContext>((option) =>
{
    
    option.UseSqlServer(connectionString);
});

// There is no relation between tables
builder.Services.AddDbContext<NoRelationDBContext>((option) =>
{
    option.UseSqlServer(connectionString);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseGlobalErrorHandler();

app.MapControllers();

app.Run();
