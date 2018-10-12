using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeepSpace.Contracts;
using DeepSpace.Core;
using DeepSpace.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeepSpace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IShipManager, ShipManager>()
                //.AddTransient<IShipDataAccess, ShipDataAccess>()       // the data access for using an azure CosmosDB database - this should ALWAYS be configured in trunk
                .AddTransient<IShipDataAccess, FileBasedDataAccess>()  // a local data access class for local file based data access
                .AddMvc();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
