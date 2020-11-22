using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nuages.QueueService.SQS;
using Nuages.QueueService.Jobs;

namespace Demo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            services.AddDefaultAWSOptions(GetAWSOptions());
            services.AddSqsService();
            
            services.AddHostedService<JobWorkerService>();
            
            services.AddAWSService<IAmazonSimpleNotificationService>();
            
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Demo.API", Version = "v1"}); });
        }

        // ReSharper disable once InconsistentNaming
        private AWSOptions GetAWSOptions()
        {
            var awsOptions = Configuration.GetAWSOptions();
            
            //By default, the credentials informations are loaded from the profile indicated in the appsettings.json (AWS:Profile). If none is provided, it will look for a profile called default.

            //You may want to use an alternatice authentication strategy

            //To use AccessKey and SecretKey
            //awsOption.Credentials = new BasicAWSCredentials(Configuration["AWS:AccessKey"], Configuration["AWS:SecretKey"])

            //To use environment variables
            //Set AWS_ACCESS_KEY_ID & AWS_SECRET_ACCESS_KEY & AWS_REGION in your environment.
            //awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();

            //More info here https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
            
            return awsOptions;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}