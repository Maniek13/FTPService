using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Interfaces.Controllers.DbControllers
{
    internal interface IConfigurationWebController
    {
        Task<IResponseModel<bool>> AddConfiguration(string serviceName, string actionName,  HttpContext context);
        Task<IResponseModel<bool>> EditConfiguration(string serviceName, string actionNam, HttpContext context);
        Task<IResponseModel<bool>> DeleteConfiguration(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> AddActionFolder(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> EditeActionFolder(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> DeleteActionFolder(string serviceName, string actionName, HttpContext context);
    }
}
