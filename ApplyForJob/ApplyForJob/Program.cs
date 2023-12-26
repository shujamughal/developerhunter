using ApplyForJob.DbContexts;
using ApplyForJob.Repository;
using Microsoft.EntityFrameworkCore;
//using MassTransit;
//using ApplyForJob.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<JobApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq://localhost");

//        cfg.ReceiveEndpoint("ResumeId", ep =>
//        {
//            ep.Durable = false;
//            ep.AutoDelete = true;
//            ep.ConfigureConsumer<ResumeIdConsumer>(context);
//        });
//    });

//    x.AddConsumer<ResumeIdConsumer>();
//});

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
