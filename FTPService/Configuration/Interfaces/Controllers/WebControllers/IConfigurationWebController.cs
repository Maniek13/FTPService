using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;

namespace Configuration.Interfaces.Controllers.DbControllers
{
    public interface IConfigurationWebController
    {
        IResponseModel<FTPConfigurationModel> GetConfiguration(string serviceName, HttpContext context);
        Task<IResponseModel<FTPConfigurationModel>> AddConfigurationAsync(string serviceName, FTPConfigurationModel cfg, HttpContext context);
        Task<IResponseModel<bool>> EditConfigurationAsync(string serviceName, FTPConfigurationModel cfg, HttpContext context);
        Task<IResponseModel<bool>> DeleteConfigurationAsync(string serviceName, HttpContext context);
        IResponseModel<List<ServiceActionModel>> GetActionsFolders(string serviceName, HttpContext context);
        Task<IResponseModel<ServiceActionModel>> AddActionFolderAsync(string serviceName, ServiceActionModel servicesAction, HttpContext context);
        Task<IResponseModel<bool>> EditeActionFolderAsync(string serviceName, ServiceActionModel servicesAction, HttpContext context);
        Task<IResponseModel<bool>> DeleteActionFolderAsync(string serviceName, string actionName, HttpContext context);
    }
}
