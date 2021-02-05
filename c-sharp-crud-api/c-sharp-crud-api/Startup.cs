using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using c_sharp_crud_api.Data;

namespace c_sharp_crud_api
{
    public class Startup
    {
        public static TaskBoard board;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Domain"];
                options.Audience = Configuration["Auth0:Identifier"];
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "c_sharp_crud_api", Version = "v1" });
            });

            services.AddDbContext<TaskBoard>(o => o.UseInMemoryDatabase("TaskBoard"), ServiceLifetime.Singleton);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "c_sharp_crud_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var context = app.ApplicationServices.GetService<TaskBoard>();
            AddStartingTasks(context);
        }

        private static void AddStartingTasks(TaskBoard context)
        {
            var newTask = new Models.Task()
            {
                Id = 1,
                Name = "C# CRUD API Walkthrough",
                Description = "Finish a C# CRUD API tutorial and deploy with Visual Studio to Azure",
                Status = "Defined"
            };

            context.Tasks.Add(newTask);
            context.SaveChanges();
        }
    }
}
