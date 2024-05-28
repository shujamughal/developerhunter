using Microsoft.EntityFrameworkCore;
using Resume.RabbitMQ;
using Resume.Repository;
using MassTransit;
using MediatR;
using System.Reflection;
using Resume.Consumers;
using jobPosting.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost"), h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("SharedContent.Messages:JWTokenResume", ep => {
            ep.Consumer<JWTokenResumeConsumer>();
        });

        cfg.ReceiveEndpoint("SharedContent.Messages:CompanyJWTokenResume", ep => {
            ep.Consumer<CompanyJWTTokenResumeConsumer>();
        });
    });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
// Add Entity Framework Core and SQL Server support
builder.Services.AddDbContext<Resume.ResumeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IResumeRepository,
ResumeRepository>();

builder.Services.AddScoped<IResumeIdProducer, ResumeIdProducer>();

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
