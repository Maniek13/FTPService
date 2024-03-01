using AutoMapper;
using Configuration.Controllers.DbControllers;
using Domain.Controllers.WebControllers;
using FTPServiceLibrary.Helpers;
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
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();


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

FilesWebController filesWebController = new(mapper, app.Logger, new FTPRODbController(), new FTPDbController());

app.MapGet("/GetFileAsync", filesWebController.GetFileAsync)
    .WithDescription("Pobieranie pliku")
    .WithOpenApi();

app.MapGet("/GetAllActionFilesInZipFile", filesWebController.GetAllActionFilesInZipFile)
    .WithDescription("Pobieranie wszytskich plików akcji w formie zip")
    .WithOpenApi();

app.MapPost("/SendFilesAsync", filesWebController.SendFilesAsync)
    .WithDescription("Wysy³anie plików")
    .WithOpenApi()
    .DisableAntiforgery();

app.MapDelete("/DeleteFileAsync", filesWebController.DeleteFileAsync)
    .WithDescription("Usuwanie pliku")
    .WithOpenApi();

app.MapDelete("/DeleteAllActionsFilesAsync", filesWebController.DeleteAllActionsFilesAsync)
    .WithDescription("Usuwanie wszytskich plików akcji")
    .WithOpenApi();


app.Run();