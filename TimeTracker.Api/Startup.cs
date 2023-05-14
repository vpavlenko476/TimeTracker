using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Jira.Client;
using Jira.Client.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Services;
using Services.Contracts.Interfaces;
using TimeTracker.Api.Configurations;
using TimeTracker.Api.Extensions;
using TimeTracker.Api.HostedServices;
using TimeTracker.Api.MapperProfiles;
using TimeTracker.Data;

namespace TimeTracker.Api
{
    public class Startup
    {
        private IConfiguration configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeTracker.Api", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
                .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter {AllowIntegerValues = false,});
                });
            
            services.AddCustomSwagger(configuration, false, null, Assembly.GetExecutingAssembly());
            services.AddScoped<IJiraClientConfiguration, JiraClientConfiguration>();
            services.AddScoped<IJiraTaskClient, JiraTaskClient>();
            services.AddScoped<ILogTimeService, LogTimeService>();
            services.AddScoped<IGetJiraItemByPeriodService, GetJiraItemByPeriodService>();
            services.AddTransient<ISaveOrUpdateWorkingTimePeriodService, SaveOrUpdateWorkingTimePeriodService>();
            
            services.AddHostedService<JiraTasksConsumerService>();
            services.AddHostedService<JiraTasksMonitoringService>();
            services.AddMapperProfiles();
            services.AddDbContext<TimeTrackerContext>(options => options.UseNpgsql(configuration["POSTGRES_CONNECTION_STRING"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwagger(configuration, "TimeTracker.Api");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.EnsureMigrate<TimeTrackerContext>();
        }
    }
}
