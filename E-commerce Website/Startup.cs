using E_commerce_Website.Data;
using E_commerce_Website.Entites;
using E_commerce_Website.Middleware;
using E_commerce_Website.RequestHelpers;
using E_commerce_Website.Services;
using E_commerce_Website.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace E_commerce_Website
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

            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E_commerce_Website", Version = "v1" });
                //c.DocumentFilter<RoleDocumentFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt auth header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                //c.OperationFilter<MyHeaderFilter>();
                c.ExampleFilters();

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = "E-commerce Website" + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);

               c.IncludeXmlComments(commentsFile);
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            // services.AddSwaggerExamplesFromAssemblyOf<TestExamples>();
            //services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddDbContext<StoreContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentityCore<User>(opt => opt.User.RequireUniqueEmail = true).AddRoles<Roles>().AddEntityFrameworkStores<StoreContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey((Encoding.UTF8.GetBytes(Configuration["JWTSettings:TokenKey"])))
                    };
                });
            services.AddAuthorization();
            services.AddScoped<TokenService>();
            services.AddScoped<PaymentServices>();
            services.AddScoped<ImageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            
            if (env.IsDevelopment())
            {
                /*app.UseDeveloperExceptionPage();*/
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E_commerce_Website v1"));
            }

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors(options => options.WithOrigins("http://localhost:3000", "http://localhost:4200").AllowAnyHeader().AllowCredentials().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
              //  endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
