﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
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
            
            // Add Identity services to the services container.
            //   services.AddIdentity<User, IdentityRole>()
            //   .AddEntityFrameworkStores<DataContext>()
            //     .AddDefaultTokenProviders();
            // // Add other services

            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        // cfg.RequireHttpsMetadata = false;
        // cfg.SaveToken = true;

        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Tokens:Key"],
            ValidAudience = Configuration["Tokens:Key"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Tokens:Key").Value))
        };

    });
           // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //              {
            //               options.TokenValidationParameters = new TokenValidationParameters()
            //              {
            //              ValidateIssuerSigningKey = true,
            //                 IssuerSigningKey = new SymmetricSecurityKey
            //               (Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings").Value)), 
            //                  ValidateIssuer = false,
            //                 ValidateAudience = false
            //           };
            //       });

    //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //     .AddJwtBearer(options =>
    //     {
    //       options.TokenValidationParameters = new TokenValidationParameters
    //       {
    //         ValidateIssuer = true,
    //         ValidateAudience = true,
    //         ValidateLifetime = true,
    //         ValidateIssuerSigningKey = true,
    //         ValidIssuer = Configuration["Jwt:Issuer"],
    //         ValidAudience = Configuration["Jwt:Issuer"],
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
    //       };
    //     });
               
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

          //  app.UseHttpsRedirection();
           // Add cookie-based authentication to the request pipeline.
            //app.UseIdentity();

        app.UseCors(m => m.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
         app.UseAuthentication();
         app.UseMvc();
        }
    }
}
