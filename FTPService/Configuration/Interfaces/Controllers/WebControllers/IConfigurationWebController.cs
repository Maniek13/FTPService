using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;

namespace Configuration.Interfaces.Controllers.DbControllers
{
    internal interface IConfigurationWebController
    {
        IResponseModel<FTPConfigurationModel> GetConfiguration(string serviceName, HttpContext context);
        Task<IResponseModel<FTPConfigurationModel>> AddConfiguration(string serviceName, FTPConfigurationModel cfg, HttpContext context);
        Task<IResponseModel<bool>> EditConfiguration(string serviceName, FTPConfigurationModel cfg, HttpContext context);
        Task<IResponseModel<bool>> DeleteConfiguration(string serviceName, HttpContext context);
        Task<IResponseModel<List<ServiceActionModel>>> GetActionsFolders(string serviceName, HttpContext context);
        Task<IResponseModel<ServiceActionModel>> AddActionFolder(string serviceName, ServiceActionModel servicesAction, HttpContext context);
        Task<IResponseModel<bool>> EditeActionFolder(string serviceName, ServiceActionModel servicesAction, HttpContext context);
        Task<IResponseModel<bool>> DeleteActionFolder(string serviceName, string actionName, HttpContext context);
    }
}
