using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using school_management_system_API.Context;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;
using System.Text.Json.Serialization;

namespace school_management_system_API
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
            services.AddDbContext<DataBaseContext>(options =>
            {
                String connectionString = Configuration.GetConnectionString("SchoolDBConnection");

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<SchoolService>()
                .AddScoped<StudentService>()
                .AddScoped<AddressService>();

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); //avoid loop

            services.AddControllers()
                .AddOData(options => options.AddRouteComponents(GetEdmModel()).Expand().EnableQueryFeatures(50).SkipToken().SetMaxTop(50).Select().Filter().OrderBy().Count());
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management System API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management System API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder odataBuilder = new();

            //odataBuilder.EnableLowerCamelCase(NameResolverOptions.ProcessDataMemberAttributePropertyNames);

            odataBuilder.EntitySet<School>("School").EntityType.Abstract().HasKey(x => x.Id);

            odataBuilder.EntitySet<Student>("Student").EntityType.HasKey(x => x.Id);

            odataBuilder.EntitySet<AddressBase>("Address").EntityType.HasKey(x => x.Id);

            return odataBuilder.GetEdmModel();
        }
    }
}
