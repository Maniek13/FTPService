using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Interfaces.Controllers.WebControllers
{
    public interface IFilesWebController
    {
        Task<IResponseModel<bool>> SendFilesAsync(string serviceName, string actionName, [FromForm] IFormFileCollection files, HttpContext context);
        Task<IResponseModel<FormFileCollection>> GetFilesAsync(string serviceName, string actionNam, HttpContext context);
        Task<IResponseModel<FormFile>> GetFileAsync(string serviceName, int id, HttpContext context);
        Task<IResponseModel<bool>> DeleteAllActionsFilesAsync(string serviceName, string actionName, HttpContext context);
        Task<IResponseModel<bool>> DeleteFileAsync(string serviceName, string actionName, string fileName, HttpContext context);
    }
}
