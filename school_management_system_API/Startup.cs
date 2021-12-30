using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using school_management_system_API.Context;
using school_management_system_API.Models;
using school_management_system_API.Models.Authentication;
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

            
            TokenConfigurationsModel tokenConfigurations = new();

            new ConfigureFromConfigurationOptions<TokenConfigurationsModel>(Configuration.GetSection("Authentication")).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(bearerOptions =>
                {
                    TokenValidationParameters paramsValidation = bearerOptions.TokenValidationParameters;

                    paramsValidation.IssuerSigningKey = tokenConfigurations.SecurityKey;

                    paramsValidation.ValidAudience = tokenConfigurations.Audience;
                    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            services.AddCors(options =>
             {
                 options.AddPolicy("AllowSite", builder => builder.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
             });

            services.AddAuthorization(options => options.AddPolicy("ODataServiceApiPolicy", x => x.RequireClaim("scope", "ScopeAspNetCoreODataServiceApi")));

            services.AddControllers()
                .AddOData(options => options.AddRouteComponents(GetEdmModel()).Expand().EnableQueryFeatures(50).SkipToken().SetMaxTop(50).Select().Filter().OrderBy().Count())
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); //avoid loop


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management System API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer accessToken'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
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

            app.UseCors("AllowAll");

            app.UseAuthentication();

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
