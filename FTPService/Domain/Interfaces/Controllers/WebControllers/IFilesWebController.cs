using Microsoft.AspNetCore.Mvc;
using FTPServiceLibrary.Interfaces.Models;

namespace Domain.Interfaces.Controllers.WebControllers
{
    internal interface IFilesWebController
    {
        Task<IResponseModel<bool>> SendFileAsync(string serviceName, string actionName, [FromForm] IFormFileCollection files, HttpContext context);
        Task<IResponseModel<FormFileCollection>> GetFilesAsync(string serviceName, string actionNam, HttpContext context);
        Task<IResponseModel<FormFile>> GetFileAsync(string serviceName, int id, HttpContext context);
        Task<IResponseModel<bool>> DeleteAllActionsFiles(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> DeleteFile(string serviceName, string actionName, string fileName, HttpContext context);
    }
}
