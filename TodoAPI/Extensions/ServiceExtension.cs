using Contracs;
using LoggerService;
using Service;
using Service.Contracts;
using Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Asp.Versioning;
using System.Threading.RateLimiting;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders());
            });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSqlite<RepositoryContext>(configuration.GetConnectionString("DefaultConnection"));

        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));

        public static void AddCustomMediaType(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.vinix.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.vinix.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter != null)
                {

                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.vinix.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.vinix.apiroot+xml");
                }

            });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            }).AddMvc();
        }

        public static void ConfigureResponseCache(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }

        public static void ConfigureOutputCache(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.AddPolicy("120SecondsDurationPolicy", builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(120));
                });
            });
        }


        public static void ConfigureRateLimiting(this IServiceCollection services)
        {

            services.AddRateLimiter(opt =>
            {

                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        Window = TimeSpan.FromMinutes(1)
                    });
                });


                opt.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).");
                    }

                    else
                    {
                        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
                    }
                };
            });
        }


        public static void ConfigureIdentity(this IServiceCollection services)
        {

            services.AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();

        }


        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("TODOAPI_SECRET");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

    }

}
