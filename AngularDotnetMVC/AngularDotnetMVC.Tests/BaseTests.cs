using System;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.File;

namespace AngularDotnetMVC.Tests;

public class BaseTests
{
    protected WebApplication application;

    public BaseTests(string environment)
    {

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

        var builder = WebApplication.CreateBuilder();

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services.RegisterApplication(builder.Configuration);
        builder.Services.RegisterFileStorage(builder.Configuration);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html"); ;

        // Serve multiple angular spa from a single core web application RSS
        // https://forums.asp.net/t/2156516.aspx?Serve+multiple+angular+spa+from+a+single+core+web+application

        //app.Run();

        application = app;
    }
}


public static class WebHostExtensionMethods
{
    public static T GetService<T>(this WebApplication webApplication)
    {
        var service = (T)webApplication.Services.GetRequiredService(typeof(T));

        return service;
    }
}