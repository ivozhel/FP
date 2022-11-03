using FluentValidation;
using FluentValidation.AspNetCore;
using PearlyWhites.Host.Extensions;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi;
using PearlyWhites.Host.Extensions.SwaggerExamples;
using MediatR;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.BL.CommandHandlers;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Logging.AddSerilog(logger);

builder.Services.AddMediatR(typeof(AddTreatmentHandler).Assembly);

// Add services to the container.
builder.Services.RegisterRepos()
    .RegisterServices()
    .AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters(); 
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<PatientSwaggerExample>();  

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
