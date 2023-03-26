using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RepositoryLayer.Context;
using RepositoryLayer.IRepo;
using RepositoryLayer.Repo;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.IServices;
using ServiceLayer.Mapping;
using ServiceLayer.Services;
using SharedDTO;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Luftborn
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
            #region Enable Cors
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            #endregion

            #region Context
            services.AddDbContext<ApplicationDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("saqayaCon"),
                sqlServerOptionsAction: sqlOptions =>
               {
                   sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                   sqlOptions.EnableRetryOnFailure(maxRetryCount: 15,
                       maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
               }
                ).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            #endregion
            JsonSerializerOptions optionss = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            #region Json Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
            #endregion

            //addSwaggerDocument
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Saqaya";
            });

            services.AddControllers();

            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            #region AutoMapper
            services.AddAutoMapper(typeof(UserMapping).Assembly);
            #endregion

            #region Repo AddScoped
            Type[] repositories = Assembly.Load(typeof(UserRepo).Assembly.GetName()).GetTypes().Where(r => r.IsClass && r.Name.EndsWith("Repo")).ToArray();
            Type[] iRepositories = Assembly.Load(typeof(IUserRepo).Assembly.GetName()).GetTypes().Where(r => r.IsInterface && r.Name.EndsWith("Repo")).ToArray();
            foreach (var repoInterface in iRepositories)
            {
                System.Type classType = repositories.FirstOrDefault(r => repoInterface.IsAssignableFrom(r));
                if (classType != null)
                {
                    services.AddScoped(repoInterface, classType);
                }
            }

            #endregion

            #region Services AddScoped
            Type[] appservices = Assembly.Load(typeof(UserService).Assembly.GetName()).GetTypes().Where(r => r.IsClass && r.Name.EndsWith("Service")).ToArray();
            Type[] iappservices = Assembly.Load(typeof(IUserService).Assembly.GetName()).GetTypes().Where(r => r.IsInterface && r.Name.EndsWith("Service")).ToArray();
            foreach (var repoInterface in iappservices)
            {
                System.Type classType = appservices.FirstOrDefault(r => repoInterface.IsAssignableFrom(r));
                if (classType != null)
                {
                    services.AddScoped(repoInterface, classType);
                }
            }
            #endregion

            #region UnitOfWord
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            //Enable Cors
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
