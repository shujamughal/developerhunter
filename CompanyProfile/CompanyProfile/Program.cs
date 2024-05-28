using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CompanyProfile.Repository;
using CompanyProfile.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using CompanyProfile.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost"), h => {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CompanyContext>()
.AddDefaultTokenProviders();
builder.Services.AddDbContext<CompanyContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<CompanyContext, CompanyContext>();
builder.Services.AddTransient<ICompanyRepository,CompanyRepository>();
builder.Services.AddTransient<ICompanyProfileRepository, CompanyProfileRepository>();
builder.Services.AddTransient<ICompanyDepartmentsRepository, CompanyDepartmentsRepository>();
builder.Services.AddTransient<ICompanyInsightsRepository, CompanyInsightsRepository>();
builder.Services.AddTransient<ICompanyReviewRepository, CompanyReviewRepository>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthorization();
app.MapControllers();
app.Run();
