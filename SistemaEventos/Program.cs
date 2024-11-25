
using BLL.Contrato;
using DAL.Contrato;
using Implementacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SistemaEventos.Comun;
using System.Text;

namespace SistemaEventos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Inyección de Dependencias
            builder.Services.AddSingleton<Utilidades>();
            builder.Services.AddScoped<IUsuarioDAL, UsuarioDAL>();
            builder.Services.AddScoped<IEventoDAL, EventoDAL>();
            builder.Services.AddScoped<IUsuarioBLL, UsuarioBLL>();
            builder.Services.AddScoped<IEventoBLL, EventoBLL>();


            //JSON Web Token (JWT) configuration
            builder.Services.AddAuthentication(configuration =>
            {
                configuration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configuration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configuration =>
            {
                configuration.RequireHttpsMetadata = false;
                configuration.SaveToken = true;
                configuration.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false, //valida q las apps externas puedan usar la URL donde se encuentra nuestra Api
                    ValidateAudience = false, //quienes pueden acceder a nuestra Api
                    ValidateLifetime = true, //valida el tiempo de vida del Token
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!))
                };
            });


            //Habilitar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PolicyCORS", app =>
                {
                    app
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors("PolicyCORS");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

    }
}
