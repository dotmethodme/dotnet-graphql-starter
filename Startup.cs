using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;


namespace PersonalCrm
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Config
            var databaseSettings = Configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(databaseSettings);
            var jwtSettings = Configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(jwtSettings);

            // Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IReminderService, ReminderService>();

            // Authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                        ValidateIssuer = true,
                        ValidAudience = jwtSettings.GetValue<string>("Audience"),
                        ValidateAudience = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Secret"))),
                        ValidateIssuerSigningKey = true
                    };
                });

            services
                .AddGraphQLServer()
                .AddQueryType(q => q.Name("Query"))
                .AddType<UserQuery>()
                .AddType<ReminderQuery>()

                .AddMutationType(q => q.Name("Mutation"))
                .AddType<UserMutation>()
                .AddType<ReminderMutation>()

                .AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseMiddleware<AuthMiddleware>();

            app.UseAuthentication();

            app.UseRouting().UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

        }
    }
}
