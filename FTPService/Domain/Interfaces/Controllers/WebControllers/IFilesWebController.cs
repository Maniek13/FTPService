using Microsoft.AspNetCore.Mvc;
using FTPServiceLibrary.Interfaces.Models;

namespace Domain.Interfaces.Controllers.WebControllers
{
    internal interface IFilesWebController
    {
        Task<IResponseModel<bool>> SendFileAsync(string serviceName, string actionName, [FromForm] IFormFileCollection file, HttpContext context);
        Task<IResponseModel<bool>> GetFilesAsync(string serviceName, string actionNam, HttpContext context);
        Task<IResponseModel<bool>> GetFileAsync(string serviceName, int id, HttpContext context);
        Task<IResponseModel<bool>> DeleteFilesAll(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> DeleteFile(string serviceName, string actionName, HttpContext context);
    }
}
