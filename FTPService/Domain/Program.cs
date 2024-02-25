using Configuration.Controllers.DbControllers;
using Configuration.Data;
using Domain.Controllers.WebControllers;
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

FilesWebController filesWebController = new(app.Logger, new FTPRODbController(), new FTPDbController());
app.MapGet("/SendFileAsync", filesWebController.SendFileAsync)
    .WithDescription("Wysy³anie pliku")
    .WithOpenApi();

app.MapPost("/GetFileAsync", filesWebController.GetFileAsync)
    .WithDescription("Pobieranie pliku")
    .WithOpenApi();

app.MapPut("/GetFilesAsync", filesWebController.GetFilesAsync)
    .WithDescription("Pobieranie plików akcji")
    .WithOpenApi();

app.MapDelete("/DeleteFile", filesWebController.DeleteFile)
    .WithDescription("Usuwanie pliku")
    .WithOpenApi();

app.MapDelete("/DeleteAllActionsFiles", filesWebController.DeleteAllActionsFiles)
    .WithDescription("Usuwanie wszytskich plików akcji")
    .WithOpenApi();

app.Run();