using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Domain.Interfaces;

namespace REST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; options.JsonSerializerOptions.WriteIndented = true; });            
            services.AddDbContext<DemoComptesContext>(context => { context.UseInMemoryDatabase("DB_REST"); });
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            //TODO serviceCollection
            /*services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; options.JsonSerializerOptions.WriteIndented = true; });
            var serviceCollection = new ServiceCollection()
                .AddDbContext<DemoComptesContext>(context => { context.UseInMemoryDatabase("DB_REST"); })
                .AddScoped<IAccountsRepository, AccountsRepository>()
                .AddScoped<ITransactionsRepository, TransactionsRepository>();
                .BuildServiceProvider();*/
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
