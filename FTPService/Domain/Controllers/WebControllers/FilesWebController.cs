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


                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName);

                for (int i = 0; i < files.Count; ++i)
                {
                    await FTPHelper.SendFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path, files[i]);

                    var file = new FilesDbModel()
                    {
                        ServiceActionId = action.Id,
                        Name = files[i].FileName,
                        Path = serviceName + "//" + action.Path + "//" + files[i].FileName,
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
        public async Task<IResult> GetAllActionFilesInZipFile(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName);
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");

                var filesInActionName = _ftpRODbController.GetActionFiles(action.Id);
                string uniqeId = Guid.NewGuid().ToString();
                var pathToZipFile = await FTPHelper.GetPathToZipArchiweWithActionDirectoryFiles(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path, action.ActionName, uniqeId);

                var bytes = File.ReadAllBytes(pathToZipFile);
                var fileName = $"{actionName}.zip";
                var pathToDir = pathToZipFile.Replace(fileName, "");
                File.Delete(pathToZipFile);
                Directory.Delete(pathToDir);


                return Results.File(bytes, null, fileName);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return Results.NotFound(ex.Message);
            }
        }


        public async Task<IResult> GetFileAsync(string serviceName, int id, HttpContext context)
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

                var fileToDownload = await FTPHelper.GetFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path, file.Name);

                return Results.File(fileToDownload, null, file.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return Results.NotFound(ex.Message);
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
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName);




                await FTPHelper.DeleteDirectory(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path);

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
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName);

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
