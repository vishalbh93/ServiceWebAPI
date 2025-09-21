using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service.Generativelogic.AccountService;
using Service.Generativelogic.Interfaces;
using System.Text;

namespace Servie.ServiceWebAPI
{
    public class RegisterServices
    {
        #region Public Methods
        public static void RegisterComponent(WebApplicationBuilder builder)
        {
            RegisterDataService(builder.Services, builder.Configuration);
            RegisterJWTService(builder);
            RegisterCORSservice(builder);
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
        }
        #endregion

        #region Private Methods
        private static void RegisterDataService(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IDataService>(s =>new DataService());
            services.AddScoped<IAccountService>(s => new AccountService(configuration));
        }

        private static void RegisterJWTService(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
            });
        }

        private static void RegisterCORSservice(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });
        }

        #endregion
    }
}
