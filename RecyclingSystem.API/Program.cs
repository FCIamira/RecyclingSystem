﻿using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecyclingSystem.Application.Feature.Account.Commands;
using RecyclingSystem.Application.Feature.PickupRequest.Queries.GetAllPickupRequests;
using RecyclingSystem.Application.Interfaces;
using RecyclingSystem.Application.Mapping;
using RecyclingSystem.Application.Services;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using RecyclingSystem.Infrastructure.Repository;
using System.Security.Claims;
using System.Text;

namespace RecyclingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logging
            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            // DbContext
            builder.Services.AddDbContext<RecyclingDbContext>(options =>

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );



            // Repositories
            builder.Services.AddScoped(typeof(IGenericRepo<,>), typeof(GenericRepo<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Controllers
            builder.Services.AddControllers();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            builder.Services.AddScoped<IEmployeeAvailabilityService, EmployeeAvailabilityService>();

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<RecyclingDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Iss"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Aud"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}") ?? new List<string>();
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogInformation("✅ JWT Validated. Claims: {Claims}", string.Join(", ", claims));
                        logger.LogInformation("Principal Identity Authenticated: {IsAuthenticated}", context.Principal?.Identity?.IsAuthenticated);
                        logger.LogInformation("HttpContext.User Authenticated: {IsAuthenticated}", context.HttpContext.User?.Identity?.IsAuthenticated);
                        context.HttpContext.User = context.Principal ?? context.HttpContext.User;
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(" Authentication Failed", context.Exception.Message);
                        logger.LogInformation("Authorization Header", context.HttpContext.Request.Headers["Authorization"]);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("❌ JWT Challenge: {Error} - {ErrorDescription}", context.Error, context.ErrorDescription);
                        logger.LogInformation("Authorization Header: {Header}", context.HttpContext.Request.Headers["Authorization"]);
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogInformation("Received Token: {Token}", context.Token ?? "(null)");
                        logger.LogInformation("Authorization Header: {Header}", context.HttpContext.Request.Headers["Authorization"]);
                        return Task.CompletedTask;
                    }
                };
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(PickupRequestProfile).Assembly);

            // MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPickupRequestsQuery).Assembly));

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = "ITI Project"
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                   // Type = SecuritySchemeType.Http,
                     Type = SecuritySchemeType.ApiKey,

                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
                });

              swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
                //config.UseSqlServerStorage(builder.Configuration.GetConnectionString("MarlyCS"));
            });

            builder.Services.AddHangfireServer();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseHangfireDashboard(); // هيفتح Dashboard على /hangfire
            app.Run();
        }
    }
}
