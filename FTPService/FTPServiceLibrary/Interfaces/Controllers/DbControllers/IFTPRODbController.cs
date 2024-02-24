using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models.DbModels;

namespace FTPServiceLibrary.Interfaces.DbControllers
{
    public interface IFTPRODbController
    {
        ServicesPermisionsDbModel GetPermision(string serviceName);
        ConfigurationDbModel GetFTPConfiguration(int serviceId);
        List<ServiceActionDbModel> GetServiceActions(int serviceId);
        FilesDbModel GetFile(int id);
        ServiceActionDbModel GetServiceAction(int serviceId, string actionName);
        ServiceActionDbModel GetServiceAction(int actionId);
        List<FilesDbModel> GetActionFiles(int actionId);
    }
}
