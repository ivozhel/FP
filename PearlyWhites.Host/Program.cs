using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using PearlyWhites.BL.CommandHandlers;
using PearlyWhites.BL.Services.HostedServices;
using PearlyWhites.Host.Extensions;
using PearlyWhites.Host.Extensions.SwaggerExamples;
using PearlyWhites.Host.Middleware;
using PearlyWhites.Models.Models.Configurations;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.Filters;

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
builder.Services.AddHealthChecks();

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

builder.Services.Configure<KafkaConfiguration>(
    builder.Configuration.GetSection(nameof(KafkaConfiguration)));

builder.Services.AddHostedService<ReportConsumeHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.RegisterHealthChecks();
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();
