using FluentValidation;
using InvoiceProcessor.Api.Behaviours;
using InvoiceProcessor.Api.Middleware;
using InvoiceProcessor.Api.Services;
using InvoiceProcessor.Application.Documents.Commands.UploadDocument;
using InvoiceProcessor.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UploadDocumentCommand).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(UploadDocumentCommand).Assembly);

// Pipeline behaviour
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.AddHttpContextAccessor();
// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IFileValidator, FileValidator>();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
