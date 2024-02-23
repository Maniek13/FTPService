using Microsoft.AspNetCore.Mvc;

namespace Domain.Interfaces.Controllers.DbControllers
{
    internal interface FilesController
    {
        Task SendFileAsync(string serviceName, string actionName, [FromForm] IFormFileCollection file, HttpContext context);
        Task GetFilesAsync(string serviceName, string actionNam, HttpContext context);
        Task DeleteFilesAll(string serviceName, string actionName, HttpContext context);
        Task DeleteFile(string serviceName, string actionName, HttpContext context);
    }
}
