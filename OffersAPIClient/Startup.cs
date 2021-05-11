using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffersAPIClient.Business;
using OffersAPIClient.Business.Interface;
using OffersAPIClient.Common.Service;
using OffersAPIClient.Common.Service.Interface;
using OffersAPIClient.Middleware;
using OffersAPIClient.Repository;
using OffersAPIClient.Repository.Interface;
using OffersAPIClient.Repository.ThirdPartyClients;

namespace OffersAPIClient
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
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMvc().AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();

            services.AddTransient<IOffersService, OffersService>();
            services.AddTransient<IOffersRepository, OffersRepository>();
            
            services.AddTransient<IGetClientOffer, RX2GoAPIClient>();
            services.AddTransient<IGetClientOffer, FedXAPIClient>();
            services.AddTransient<IGetClientOffer, PremierAPIClient>();

            services.AddTransient<IRestClient, RestClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offers Client API's V1");
            });

            app.UseRouting();
            app.ConfigureCustomExceptionMiddleware();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
