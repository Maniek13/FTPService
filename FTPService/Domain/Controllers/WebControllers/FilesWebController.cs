using Domain.Interfaces.Controllers.WebControllers;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers.WebControllers
{
    public class FilesWebController(ILogger logger, IFTPRODbController fTPRODbController, IFTPDbController fTPDbController) : IFilesWebController
    {
        readonly ILogger _logger = logger;
        readonly IFTPRODbController _ftpRODbController = fTPRODbController;
        readonly IFTPDbController _ftpDbController = fTPDbController;

        public async Task<IResponseModel<bool>> SendFileAsync(string serviceName, string actionName, [FromForm] IFormFileCollection files, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id);
                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                for(int i = 0; i<files.Count; ++i)
                {
                    await FTPHelper.SendFile(ConversionHelper.ConveretToFtpConfuguration(cfg), serviceName, actionName, files[i]);

                    var file = new FilesDbModel()
                    {
                        ServiceActionId = action.Id,
                        Name = files[i].Name,
                        Path = serviceName + "//" + actionName + "//" + files[i].FileName,
                    };

                    await _ftpDbController.AddFile(file);
                }

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok",
                };
            }
            catch(Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<FormFileCollection>> GetFilesAsync(string serviceName, string actionNam, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                throw new NotImplementedException();

                return new ResponseModel<FormFileCollection>()
                {
                    Data = null,
                    Message = "ok",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return new ResponseModel<FormFileCollection>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<FormFile>> GetFileAsync(string serviceName, int id, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id);
                var file = _ftpRODbController.GetFile(id);
                var action = _ftpRODbController.GetServiceAction(file.ServiceActionId);

                IFormFile f = await FTPHelper.GetFile(ConversionHelper.ConveretToFtpConfuguration(cfg), serviceName, action.ActionName, file.Name);

                return new ResponseModel<FormFile>()
                {
                    Data = (FormFile)f,
                    Message = "ok",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return new ResponseModel<FormFile>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> DeleteAllActionsFiles(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id);
                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                await FTPHelper.DeleteAllFiles(ConversionHelper.ConveretToFtpConfuguration(cfg), serviceName, actionName);

                var files = _ftpRODbController.GetActionFiles(action.Id);

                for(int i = 0; i < files.Count; ++i)
                {
                    _ftpDbController.DeleteFile(files[i].Id);
                }

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> DeleteFile(string serviceName, string actionName, string fileName, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id);
                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                await FTPHelper.DeleteFile(ConversionHelper.ConveretToFtpConfuguration(cfg), serviceName, actionName, fileName);
                await _ftpDbController.DeleteFile(action.Id, fileName);

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
