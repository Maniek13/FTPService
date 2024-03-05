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
        readonly SemaphoreSlim _semaphore = new(1, 1);
        public async Task<IResponseModel<bool>> SendFilesAsync(string serviceName, string actionName, [FromForm] IFormFileCollection files, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("Brak konfiguracji");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName) ?? throw new Exception($"Serwis nie posiada akcji: {actionName}");

                for (int i = 0; i < files.Count; ++i)
                {
                    await _semaphore.WaitAsync();
                    await FTPHelper.SendFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path, files[i]);

                    if (_ftpRODbController.GetFile(action.Id, files[i].FileName) != null)
                        break;

                    var file = new FileDbModel()
                    {
                        ServiceActionId = action.Id,
                        Name = files[i].FileName,
                        Path = Path.Combine(serviceName, action.Path, files[i].FileName)
                    };

                    await _ftpDbController.AddFileAsync(file);
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
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task<IResult> GetAllActionFilesInZipFile(string serviceName, string actionName, HttpContext context)
        {
            string pathToZipFile = string.Empty, pathToDir = string.Empty;
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName) ?? throw new Exception($"Serwis nie posiada akcji: {actionName}");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("Brak konfiguracji");

                string uniqeId = Guid.NewGuid().ToString();
                pathToZipFile = await FTPHelper.CreateZipArchiveWithActionDirectoryFiles(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path, action.ActionName, uniqeId);

                var bytes = File.ReadAllBytes(pathToZipFile);
                var fileName = $"{actionName}.zip";
                pathToDir = pathToZipFile.Replace(fileName, string.Empty);

                return Results.File(bytes, null, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                return Results.NotFound(ex.Message);
            }
            finally
            {
                if (string.IsNullOrEmpty(pathToZipFile))
                    File.Delete(pathToZipFile);
                if (string.IsNullOrEmpty(pathToDir))
                    Directory.Delete(pathToDir);
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
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("Brak konfiguracji");
                var file = _ftpRODbController.GetFile(id) ?? throw new Exception($"Brak pliku o id: {id}");
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
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("Brak konfiguracji");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName) ?? throw new Exception($"Serwis nie posiada akcji: {actionName}");


                await FTPHelper.DeleteDirectory(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, action.Path);

                var files = _ftpRODbController.GetActionFiles(action.Id);

                for (int i = 0; i < files.Count; ++i)
                {
                    await _ftpDbController.DeleteFileAsync(files[i].Id);
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
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("Brak konfiguracji");
                var action = _ftpRODbController.GetServiceAction(permisions.Id, actionName) ?? throw new Exception($"Serwis nie posiada akcji: {actionName}");

                await FTPHelper.DeleteFile(_mapper.Map<FTPConfigurationModel>(cfg), serviceName, actionName, fileName);
                await _ftpDbController.DeleteFileAsync(action.Id, fileName);

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
