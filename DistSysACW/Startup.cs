﻿using System;
using DistSysACW.Middleware;
using DistSysACW.Models;
using DistSysACW.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistSysACW
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
            services.AddDbContext<UserContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddMvc(options => {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add(new Filters.AuthFilter());})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<Middleware.AuthMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionStatusCodeHandler();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
