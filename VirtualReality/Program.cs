using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VirtualReality.Context;
using VirtualReality.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddIdentityInfrastructure(config);
builder.Services.AddDbContext<IdentityContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("Estate")));

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc(
"v1", new OpenApiInfo { Title = "Virtual Reality API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(
    c=> {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Virtual Reality API V1");
        }) ;

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();