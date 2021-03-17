using System.IO;
using Checkout.Challenge.BankClient;
using Checkout.Challenge.BankClient.Config;
using Checkout.Challenge.Repository;
using Checkout.Challenge.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Checkout.Challenge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: true, true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddHttpClient();
            services.AddControllers();
            services.AddAutoMapper(typeof(Checkout.Challenge.Api.MappingProfile), typeof(Checkout.Challenge.Services.MappingProfile), typeof(Checkout.Challenge.BankClient.MappingProfile));
            var bankClientSettings = new BankClientSettings()
                                               {
                                                  BaseUrl = Configuration["BankApiUrl"],
                                               };
            services.AddSingleton<IBankClientSettings>(bankClientSettings);

            services.AddScoped<IBankClient, BankClient.BankClient>();
            var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;

            if(directoryInfo != null)
            {
                var directory = directoryInfo.FullName;

                services.AddScoped<IRepository>(s => new Repository.Repository($"{directory}\\{Configuration["DataPath"]}"));
            }

            services.AddScoped<IPaymentService, PaymentService>();
            
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                             {
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout Challenge Payment API");
                             });


            loggerFactory.AddFile("logs\\{Date}.log");
        }
    }
}

