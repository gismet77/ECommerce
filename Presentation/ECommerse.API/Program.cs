
using ECommerce.Application;
using ECommerce.Application.Validators.Products;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Enums;
using ECommerce.Infrastructure.Filters;
using ECommerce.Infrastructure.Services.Storage.Azure;
using ECommerce.Infrastructure.Services.Storage.Local;
using ECommerce.Persistence;
using ECommerce.SignalR;
using ECommerce.SignalR.Hubs;
using ECommerse.API.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerse.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor(); //Client-den gelen request neticesinde yaranan HttpContext obyektine qatmanlardaki class-lar uzerinden(business logicden) elcatanligimiza imkan veren servisdir.
            // Add services to the container.
            builder.Services.AddPersistenceServices(); //IoC
            builder.Services.AddInfrastructureServices(); //IoC
            builder.Services.AddApplicationServices(); //IoC
                                                       //builder.Services.AddStorage(StorageType.Azure);  IoC proqram.cs deki versiya 2(tovsiyye olunmur)
            builder.Services.AddSignalRServices();
            builder.Services.AddStorage<LocalStorage>(); //  IoC proqram.cs deki versiya 1
                                                         //builder.Services.AddStorage<AzureStorage>();  //(yuklediyimiz fayllar azure-de yuklenecek)

            //builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
            //policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn("user_name", SqlDbType.VarChar)
                }
            };

            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            };

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                 .WriteTo.MSSqlServer(
                     connectionString: builder.Configuration.GetConnectionString("SqlServer"),
                     sinkOptions: sinkOptions,
                     columnOptions: columnOptions,
                     restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                 )
                 .Enrich.FromLogContext()
                 .MinimumLevel.Information()
                 .CreateLogger();

            // Serilog-u ASP.NET host sisteminə tətbiq edirik
            builder.Host.UseSerilog();
 
             
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
             
            builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()) // ValidationFilteri dovreye qosuruq
                .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Admin", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateAudience = true, //Yaradilacaq tokenin deyerini kimlerin hansi saytlarin istifade edeceyini yoxlayir
                        ValidateIssuer = true, // Yaradilmis tokenin kim payladigini yoxlayir (meselen bizde API layeri)
                        ValidateLifetime = true, // yaradilacaq tokenin vaxtini yoxlayir
                        ValidateIssuerSigningKey = true, // yaradilacaq tokenin proqramimiza aid bir deyer oldugunu yoxlayan security.key dogrulayir


                        ValidAudience = builder.Configuration["Token:Audience"],
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
                        NameClaimType = ClaimTypes.Name // JWT uzerinde Name claimn qarsiliginda gelen deyeri User.Identity.Name propertysinden elde edirik.
                    };

                });// JWT ucun bunu elave edirik

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
            app.UseStaticFiles(); // wwwroot path-ini istifade elemeke ucun bu midlware yazilir

            app.UseSerilogRequestLogging();
            app.UseHttpLogging();
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Middleware əlavə olunur — hər HTTP request daxil olanda bu kod icra olunacaq
            app.Use(async (context, next) =>
            {
                // İstifadəçinin login olub-olmadığını yoxlayırıq
                // Əgər login olubsa, istifadəçi adını götürürük
                // Əks halda, 'Anonymous' təyin olunur
                var username = context.User?.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name  // daxil olmuş istifadəçinin adı (məsələn: admin)
                    : "Anonymous";                 // daxil olmayıbsa, 'Anonymous' olaraq qeyd edirik

                // Serilog-un log kontekstinə 'user_name' adlı property əlavə olunur
                // Bu property SQL Server-ə və ya digər log yerlərinə göndəriləcək                                  
                LogContext.PushProperty("user_name", username);
                // Növbəti middleware və ya endpoint-ə davam edir
                await next();
            });


            app.MapControllers();
            app.MapHubs();

            app.Run();
        }
    }
}
