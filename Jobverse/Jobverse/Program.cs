using MassTransit;
using Jobverse.Consumers;

using System.Net.Http;
using DinkToPdf;
using DinkToPdf.Contracts;
using Jobverse.Models;
using Jobverse.Models.Resume.Resume;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using Jobverse.Services;
using System.Text;
using Microsoft.AspNetCore.CookiePolicy;

namespace Jobverse
{
    public class Program
    {
        private static byte[] GenerateEncryptionKey()
        {
            using (var deriveBytes = new Rfc2898DeriveBytes("Jobverse", saltSize: 16, iterations: 1000))
            {
                return deriveBytes.GetBytes(32);
            }
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ErrorViewModel>();
            builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
            builder.Services.AddTransient<PdfService>();

            var encryptionKey = GenerateEncryptionKey();

            builder.Services.AddSingleton(encryptionKey);

            builder.Services.AddScoped<ITokenEncryptionService, TokenEncryptionService>();
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("SharedContent.Messages:JWToken", ep =>
                    {
                        ep.Consumer<JWTokenConsumer>();
                    });
                });
            });

            var app = builder.Build(); 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
