using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;
using RefundManagementApplication.Repositories;
using RefundManagementApplication.Services;
using System.Text;

namespace RefundManagementApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddLogging(l => l.AddLog4Net());

            #region Bearer
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
            });
            //Debug.WriteLine(builder.Configuration["TokenKey:JWT"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };

                });

            #endregion

            #region Context
            builder.Services.AddDbContext<RefundManagementContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnections")));
            #endregion

            #region Repos
            builder.Services.AddScoped<IRepository<int,Member>,MemberRepository>();
            builder.Services.AddScoped<IRepository<int,Order>,OrderRepository>();
            builder.Services.AddScoped<IRepository<int,Product>,ProductRepository>();
            builder.Services.AddScoped<IRepository<int,Refund>,RefundRepository>();
            builder.Services.AddScoped<IRepository<int,User>,UserRepository>(); 
            builder.Services.AddScoped<IRepository<int,Payment>,PaymentRepository>();
            #endregion

            #region Services
            builder.Services.AddScoped<IUserServices,UserServices>();
            builder.Services.AddScoped<ITokenServices,TokenServices>();
            builder.Services.AddScoped<IActivateServices,ActivateServices>();
            builder.Services.AddScoped<IServices<int, Order>, OrderServices>();
            builder.Services.AddScoped<IServices<int, Product>, ProductServices>();
            builder.Services.AddScoped<IServices<int, Refund>, RefundServices>();
            builder.Services.AddScoped<IServices<int, Member>, MemberServices>();
            builder.Services.AddScoped<IServices<int,User>, UserServices>();
            //builder.Services.AddScoped<IServices<int, Payment>, PaymentServices>();
            builder.Services.AddScoped<IPaymentServices,PaymentServices>();
            builder.Services.AddScoped<IOrderServices,OrderServices>();
            builder.Services.AddScoped<IRefundServices,RefundServices>();
            #endregion


            builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
            {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corspolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
