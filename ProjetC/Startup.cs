using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ProjetC.Data;
using ProjetC.Models;

namespace ProjetC
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

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
            /*
            services.AddDbContext<AccountsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AccountsContext")));
            
            services.AddDbContext<TransactionsContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TransactionsContext")));
            */
            services.AddDbContext<AccountsContext>(context =>
            {
                context.UseInMemoryDatabase("Account");
            });
            services.AddDbContext<TransactionsContext>(context =>
            {
                context.UseInMemoryDatabase("Transaction");
            });
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
                endpoints.MapControllers();
            });
        }
    }
}
