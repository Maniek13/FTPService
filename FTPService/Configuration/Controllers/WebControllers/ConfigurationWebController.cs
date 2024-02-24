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
    internal class ConfigurationWebController(ILogger logger, IFTPRODbController fTPRODbController, IFTPDbController fTPDbController) : IConfigurationWebController
    {
        ILogger _logger = logger;
        IFTPRODbController _ftpRODbController = fTPRODbController;
        IFTPDbController _ftpDbController = fTPDbController;

        public IResponseModel<FTPConfiguration> GetConfiguration(string serviceName, HttpContext context)
        {
            try
            {
                var permision = _ftpRODbController.GetPermision(serviceName);
                var cfgDb = _ftpRODbController.GetFTPConfiguration(permision.Id);
                var config = ConversionHelper.ConveretToFtpConfuguration(cfgDb);


                return new ResponseModel<FTPConfiguration>()
                {
                    Data = config,
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
                var permision = _ftpRODbController.GetPermision(serviceName);
                cfg.ServiceId = permision.Id;
                var cfgDb = await _ftpDbController.SetFTPConfigurationAsync(ConversionHelper.ConveretToConfigurationDbModel(cfg));
                var config = ConversionHelper.ConveretToFtpConfuguration(cfgDb);


                return new ResponseModel<FTPConfiguration>()
                {
                    Data = config,
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
                var permision = _ftpRODbController.GetPermision(serviceName);
                var cfgDb = await _ftpDbController.EditFTPConfigurationAsync(ConversionHelper.ConveretToConfigurationDbModel(cfg));

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
                var permision = _ftpRODbController.GetPermision(serviceName);
                await _ftpDbController.RemoveFTPConfigurationAsync(permision.Id);

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
                var permision = _ftpRODbController.GetPermision(serviceName);
                var actionsDb = _ftpRODbController.GetServiceActions(permision.Id);


                List<ServicesAction> actions = [];

                for (int i =0; i< actionsDb.Count; ++i)
                {
                    actions.Add(ConversionHelper.ConvertToServicesAction(actionsDb[i]));
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
                var permision = _ftpRODbController.GetPermision(serviceName);
                servicesAction.ServiceId = permision.Id;
                var actionsDb = await _ftpDbController.AddActionFolderAsync(ConversionHelper.ConvertToServiceActionDbModel(servicesAction));


                return new ResponseModel<ServicesAction>()
                {
                    Data = ConversionHelper.ConvertToServicesAction((ServiceActionDbModel)actionsDb),
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
                var permision = _ftpRODbController.GetPermision(serviceName);
                var actionsDb = await _ftpDbController.EditActionFolderAsync(ConversionHelper.ConvertToServiceActionDbModel(servicesAction));


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
                var permision = _ftpRODbController.GetPermision(serviceName);
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
