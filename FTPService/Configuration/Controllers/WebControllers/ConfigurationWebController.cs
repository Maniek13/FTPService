using AutoMapper;
using Configuration.Interfaces.Controllers.DbControllers;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    public class ConfigurationWebController(IMapper mapper, ILogger logger, IFTPRODbController fTPRODbController, IFTPDbController fTPDbController) : IConfigurationWebController
    {
        readonly ILogger _logger = logger;
        readonly IMapper _mapper = mapper;
        readonly IFTPRODbController _ftpRODbController = fTPRODbController;
        readonly IFTPDbController _ftpDbController = fTPDbController;

        public IResponseModel<FTPConfigurationModel> GetConfiguration(string serviceName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfg = _ftpRODbController.GetFTPConfiguration(permisions.Id) ?? throw new Exception("brak konfiguracji");

                return new ResponseModel<FTPConfigurationModel>()
                {
                    Data = _mapper.Map<FTPConfigurationModel>(cfg),
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<FTPConfigurationModel>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<FTPConfigurationModel>> AddConfigurationAsync(string serviceName, FTPConfigurationModel cfg, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateFTPConfigurationModel(cfg);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                cfg.ServiceId = permisions.Id;
                var cfgDb = await _ftpDbController.SetFTPConfigurationAsync(_mapper.Map<FTPConfigurationDbModel>(cfg)) ?? throw new Exception("brak konfiguracji");

                return new ResponseModel<FTPConfigurationModel>()
                {
                    Data = _mapper.Map<FTPConfigurationModel>(cfgDb),
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<FTPConfigurationModel>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> EditConfigurationAsync(string serviceName, FTPConfigurationModel cfg, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateFTPConfigurationModel(cfg);
                _ = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var cfgDb = await _ftpDbController.EditFTPConfigurationAsync(_mapper.Map<FTPConfigurationDbModel>(cfg));

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

        public async Task<IResponseModel<bool>> DeleteConfigurationAsync(string serviceName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
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

        public IResponseModel<List<ServiceActionModel>> GetActionsFolders(string serviceName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                var actionsDb = _ftpRODbController.GetServiceActions(permisions.Id);


                List<ServiceActionModel> actions = [];

                for (int i = 0; i < actionsDb.Count; ++i)
                {
                    actions.Add(_mapper.Map<ServiceActionModel>(actionsDb[i]));
                }

                return new ResponseModel<List<ServiceActionModel>>()
                {
                    Data = actions,
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<List<ServiceActionModel>>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<ServiceActionModel>> AddActionFolderAsync(string serviceName, ServiceActionModel servicesAction, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateServiceActionModel(servicesAction);
                var permisions = _ftpRODbController.GetPermision(serviceName) ?? throw new Exception("Serwis nie posiada pozwolenia");
                servicesAction.ServiceId = permisions.Id;
                var actionsDb = await _ftpDbController.AddActionFolderAsync(_mapper.Map<ServiceActionDbModel>(servicesAction));


                return new ResponseModel<ServiceActionModel>()
                {
                    Data = _mapper.Map<ServiceActionModel>((ServiceActionDbModel)actionsDb),
                    Message = "ok"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{GetType()} : {ex.Message}");
                context.Response.StatusCode = 400;
                return new ResponseModel<ServiceActionModel>()
                {
                    Data = null,
                    Message = ex.Message,
                };
            }
        }
        public async Task<IResponseModel<bool>> EditeActionFolderAsync(string serviceName, ServiceActionModel servicesAction, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateServiceActionModel(servicesAction);
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
        public async Task<IResponseModel<bool>> DeleteActionFolderAsync(string serviceName, string actionName, HttpContext context)
        {
            try
            {
                ValidationHelper.ValidateServiceName(serviceName);
                ValidationHelper.ValidateActionName(actionName);
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
