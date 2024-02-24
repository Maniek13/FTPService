using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Interfaces.Controllers.DbControllers
{
    internal interface IConfigurationWebController
    {
        IResponseModel<FTPConfiguration> GetConfiguration(string serviceName, HttpContext context);
        Task<IResponseModel<FTPConfiguration>> AddConfiguration(string serviceName, FTPConfiguration cfg,  HttpContext context);
        Task<IResponseModel<bool>> EditConfiguration(string serviceName, FTPConfiguration cfg, HttpContext context);
        Task<IResponseModel<bool>> DeleteConfiguration(string serviceName, HttpContext context);
        Task<IResponseModel<List<ServicesAction>>> GetActionsFolders(string serviceName, HttpContext context);
        Task<IResponseModel<ServicesAction>> AddActionFolder(string serviceName, ServicesAction servicesAction, HttpContext context);
        Task<IResponseModel<bool>> EditeActionFolder(string serviceName, ServicesAction servicesAction, HttpContext context);
        Task<IResponseModel<bool>> DeleteActionFolder(string serviceName, string actionName, HttpContext context);
    }
}
