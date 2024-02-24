using Configuration.Controllers.DbControllers;
using Configuration.Data;
using FTPServiceLibrary.Interfaces.Data;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



ConfigurationWebController configurationWebController = new(app.Logger, new FTPRODbController(new FTPServiceContextRO(AppConfig.ConnectionStringRO)), new FTPDbController(new FTPServiceContext(AppConfig.ConnectionString)));
app.MapGet("/GetConfiguration", configurationWebController.GetConfiguration)
    .WithDescription("Get coonfiguration")
    .WithOpenApi();

app.MapPost("/SetConfiguration", configurationWebController.AddConfiguration)
    .WithDescription("Set coonfiguration")
    .WithOpenApi();

app.MapPut("/UpdateConfiguration", configurationWebController.EditConfiguration)
    .WithDescription("Update coonfiguration")
    .WithOpenApi();

app.MapDelete("/DeleteConfiguration", configurationWebController.DeleteConfiguration)
    .WithDescription("Remove coonfiguration")
    .WithOpenApi();

app.MapGet("/GetActionsFolders", configurationWebController.GetActionsFolders)
    .WithDescription("Get action folder")
    .WithOpenApi();

app.MapPost("/AddActionFolder", configurationWebController.AddActionFolder)
    .WithDescription("Set action folder")
    .WithOpenApi();

app.MapPut("/UpdateActionFolder", configurationWebController.EditeActionFolder)
    .WithDescription("Update action folder")
    .WithOpenApi();

app.MapDelete("/DeleteActionFolder", configurationWebController.DeleteActionFolder)
    .WithDescription("Remove action folder")
    .WithOpenApi();

app.Run();