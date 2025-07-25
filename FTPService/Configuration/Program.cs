using AutoMapper;
using Configuration.Controllers.DbControllers;
using Configuration.Data;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json");
var config = configuration.Build();

AppConfig.SigningKey = config.GetSection("AppConfig").GetSection("SigningKey").Value;
AppConfig.ConnectionStringRO = config.GetSection("AppConfig").GetSection("ReadOnlyConnection").Value;
AppConfig.ConnectionString = config.GetSection("AppConfig").GetSection("Connection").Value;

var builder = WebApplication.CreateBuilder(args);
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<FTPServiceContextBase>(AppConfig.ConnectionString);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfig.SigningKey))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FTPServiceContextBase>();
        db.Database.Migrate();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

ConfigurationWebController configurationWebController = new(mapper, app.Logger, new FTPRODbController(), new FTPDbController());
app.MapGet("/GetConfiguration", configurationWebController.GetConfiguration)
    .WithDescription("Get coonfiguration")
    .WithOpenApi();

app.MapPost("/AddConfigurationAsync", configurationWebController.AddConfigurationAsync)
    .WithDescription("Set coonfiguration")
    .WithOpenApi();

app.MapPut("/EditConfigurationAsync", configurationWebController.EditConfigurationAsync)
    .WithDescription("Update coonfiguration")
    .WithOpenApi();

app.MapDelete("/DeleteConfigurationAsync", configurationWebController.DeleteConfigurationAsync)
    .WithDescription("Remove coonfiguration")
    .WithOpenApi();

app.MapGet("/GetActionsFolders", configurationWebController.GetActionsFolders)
    .WithDescription("Get action folders")
    .WithOpenApi();

app.MapPost("/AddActionFolderAsync", configurationWebController.AddActionFolderAsync)
    .WithDescription("Set action folder")
    .WithOpenApi();

app.MapPut("/EditeActionFolderAsync", configurationWebController.EditeActionFolderAsync)
    .WithDescription("Update action folder")
    .WithOpenApi();

app.MapDelete("/DeleteActionFolderAsync", configurationWebController.DeleteActionFolderAsync)
    .WithDescription("Remove action folder")
    .WithOpenApi();

app.Run();