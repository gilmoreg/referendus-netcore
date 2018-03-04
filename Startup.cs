namespace referendus_netcore
{
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.SpaServices.Webpack;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Startup
    {
		public IConfiguration Configuration { get; }
		public IHostingEnvironment Environment { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
			Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			// Add Authentication Services
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				options.Authority = Configuration["Auth0:Authority"];
				options.Audience = Configuration["Auth0:Audience"];
			});

			var connectionString = Environment.IsDevelopment() ?
				Configuration.GetConnectionString("psqlconnection") :
				Configuration["DATABASE_URL"];

			// Add Postgres connection
			services.AddEntityFrameworkNpgsql().AddDbContext<PsqlContext>(opt => opt.UseNpgsql(connectionString));

			// Add repositories
			// "Scoped" is one instance per request
			services.AddScoped<IUserData, SqlUserData>();
			services.AddScoped<IReferenceData, SqlReferenceData>();

			// Add MVC
			services.AddMvc();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
			app.UseAuthentication();

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
