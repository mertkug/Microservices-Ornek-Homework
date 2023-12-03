
using System.Text.Json.Serialization;
using AutoMapper;
using Inveon.Services.ProductAPI.DbContexts;
using Inveon.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Inveon.Services.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                // Add other settings as needed
            });
            
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //ioc 
            builder.Services.AddScoped<IProductRepository, ProductRepository>(); // PRoductRepository pr = new ProductRepository

            builder.Services.AddCors(o => o.AddPolicy("AllowAnyOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));
            
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5021";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClientId = "productapi";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.Scope.Add("inveon");
                    options.SaveTokens = true;

                });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}