using AutoMapper;
using Azure;
using Configuration.Interfaces.Controllers.DbControllers;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    internal class ConfigurationWebController(IMapper mapper, ILogger logger, IFTPRODbController fTPRODbController, IFTPDbController fTPDbController) : IConfigurationWebController
    {
        readonly ILogger _logger = logger;
        readonly IMapper _mapper = mapper;
        readonly IFTPRODbController _ftpRODbController = fTPRODbController;
        readonly IFTPDbController _ftpDbController = fTPDbController;

        public IResponseModel<FTPConfiguration> GetConfiguration(string serviceName, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");

                return new ResponseModel<FTPConfiguration>()
                {
                    Data = _mapper.Map<FTPConfiguration>(cfg),
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<FTPConfiguration>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<FTPConfiguration>> AddConfiguration(string serviceName, FTPConfiguration cfg,  HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                cfg.ServiceId = permisions.Id;
                var cfgDb = await _ftpDbController.SetFTPConfigurationAsync(_mapper.Map<ConfigurationDbModel>(cfg)) ?? throw new Exception("brak konfiguracji");

                return new ResponseModel<FTPConfiguration>()
                {
                    Data = _mapper.Map<FTPConfiguration>(cfgDb),
                    Message = "ok"
                };
            }
            catch(Exception ex) 
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<FTPConfiguration>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> EditConfiguration(string serviceName, FTPConfiguration cfg, HttpContext context)
        {
            try
            {
                _ = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfgDb = await _ftpDbController.EditFTPConfigurationAsync(_mapper.Map<ConfigurationDbModel>(cfg));

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<IResponseModel<bool>> DeleteConfiguration(string serviceName, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                await _ftpDbController.RemoveFTPConfigurationAsync(permisions.Id);

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
        
        public async Task<IResponseModel<List<ServicesAction>>> GetActionsFolders(string serviceName, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var actionsDb = _ftpRODbController.GetServiceActions(permisions.Id);


                List<ServicesAction> actions = [];

                for (int i =0; i< actionsDb.Count; ++i)
                {
                    actions.Add(_mapper.Map<ServicesAction>(actionsDb[i]));
                }

                return new ResponseModel<List<ServicesAction>>()
                {
                    Data = actions,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<List<ServicesAction>>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<ServicesAction>> AddActionFolder(string serviceName, ServicesAction servicesAction, HttpContext context)
        {
            try
            {
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                servicesAction.ServiceId = permisions.Id;
                var actionsDb = await _ftpDbController.AddActionFolderAsync(_mapper.Map<ServiceActionDbModel>(servicesAction));


                return new ResponseModel<ServicesAction>()
                {
                    Data = _mapper.Map<ServicesAction>((ServiceActionDbModel)actionsDb),
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<ServicesAction>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> EditeActionFolder(string serviceName, ServicesAction servicesAction, HttpContext context)
        {
            try
            {
                _ = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var actionsDb = await _ftpDbController.EditActionFolderAsync(_mapper.Map<ServiceActionDbModel>(servicesAction));


                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> DeleteActionFolder(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                _ = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                await _ftpDbController.RemoveActionFolderAsync(actionName);

                return new ResponseModel<bool>()
                {
                    Data = true,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<bool>()
                {
                    Data = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
