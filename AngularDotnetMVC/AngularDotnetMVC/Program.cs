using System.Net;
using Application;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Persistence.File;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews(configure => configure.Filters.Insert(0, new HttpResponseExceptionFilter()));

builder.Services.RegisterApplication(builder.Configuration);
builder.Services.RegisterFileStorage(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(config =>
{
    config.InvalidModelStateResponseFactory = ctx =>
    {
        //var json = System.Text.Json.JsonSerializer.Serialize();
        throw new MyApplicationException("Invalid Model State", null) {  Payload = ctx.ModelState.Values.SelectMany(p => p.Errors) };
    };
});

var app = builder.Build();

app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

    switch (exceptionHandlerPathFeature?.Error)
    {
        case FluentValidation.ValidationException validationEx:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { validationEx.Message, validationEx.Errors });

                break;
            }
        case MyApplicationException appEx:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { appEx.Message, appEx.Payload });

                break;
            }
        default:
            await context.Response.WriteAsJsonAsync(new { error = exceptionHandlerPathFeature?.Error?.Message });
            break;
    }
}));

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

app.Run();

