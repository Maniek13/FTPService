using AutoMapper;
using Domain.Interfaces.Controllers.WebControllers;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers.WebControllers
{
    public class FilesWebController(IMapper mapper, ILogger logger, IFTPRODbController fTPRODbController, IFTPDbController fTPDbController) : IFilesWebController
    {
        readonly ILogger _logger = logger;
        readonly IMapper _mapper = mapper;
        readonly IFTPRODbController _ftpRODbController = fTPRODbController;
        readonly IFTPDbController _ftpDbController = fTPDbController;

        public async Task<IResponseModel<bool>> SendFilesAsync(string serviceName, string actionName, [FromForm] IFormFileCollection files, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");


                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                for (int i = 0; i < files.Count; ++i)
                {
                    await FTPHelper.SendFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, actionName, files[i]);

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
        public async Task<IResponseModel<FormFileCollection>> GetFilesAsync(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName);
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");

                var filesInActionName = _ftpRODbController.GetActionFiles(action.Id);

                FormFileCollection files = new FormFileCollection();

                if (filesInActionName != null)
                    for (int i = 0; i < filesInActionName.Count; ++i)
                        files.Add(await FTPHelper.GetFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.ActionName, filesInActionName[i].Name));


                return new ResponseModel<FormFileCollection>()
                {
                    Data = files,
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
                ValidationHelper.ValidateServiceName(serviceName);
                if (id <= 0)
                    throw new Exception("Plik musi mieć id większe od zera");
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");
                var file = _ftpRODbController.GetFile(id);
                var action = _ftpRODbController.GetServiceAction(file.ServiceActionId);

                IFormFile f = await FTPHelper.GetFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.ActionName, file.Name);

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
        public async Task<IResponseModel<bool>> DeleteAllActionsFilesAsync(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");
                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                await FTPHelper.DeleteAllFiles(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, actionName);

                var files = _ftpRODbController.GetActionFiles(action.Id);

                for (int i = 0; i < files.Count; ++i)
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
        public async Task<IResponseModel<bool>> DeleteFileAsync(string serviceName, string actionName, string fileName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                ValidationHelper.ValidateFileName(fileName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");
                var action = _ftpRODbController.GetServiceAction(cfg.Id, actionName);

                await FTPHelper.DeleteFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, actionName, fileName);
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
