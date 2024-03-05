using FTPServiceLibrary.Models.DbModels;

namespace FTPServiceLibrary.Interfaces.DbControllers
{
    public interface IFTPRODbController
    {
        ServicesPermisionsDbModel GetPermision(string serviceName);
        FTPConfigurationDbModel GetFTPConfiguration(int serviceId);
        List<ServiceActionDbModel> GetServiceActions(int serviceId);
        FileDbModel GetFile(int id);
        FileDbModel GetFile(int serviceActionId, string fileName);
        ServiceActionDbModel GetServiceAction(int serviceId, string actionName);
        ServiceActionDbModel GetServiceAction(int actionId);
        List<FileDbModel> GetActionFiles(int actionId);
    }
}
