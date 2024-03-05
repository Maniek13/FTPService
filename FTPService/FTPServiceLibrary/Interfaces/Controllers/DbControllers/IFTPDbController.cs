using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models.DbModels;

namespace FTPServiceLibrary.Interfaces.DbControllers
{
    public interface IFTPDbController
    {
        Task<IFTPConfigurationDbModel> SetFTPConfigurationAsync(IFTPConfigurationDbModel cfg);
        Task<IFTPConfigurationDbModel> EditFTPConfigurationAsync(IFTPConfigurationDbModel cfg);
        Task RemoveFTPConfigurationAsync(int serviceId);
        Task<IServiceActionDbModel> AddActionFolderAsync(IServiceActionDbModel action);
        Task<IServiceActionDbModel> EditActionFolderAsync(IServiceActionDbModel action);
        Task RemoveActionFolderAsync(string actionName);
        Task AddFileAsync(IFilesDbModel file);
        Task DeleteFileAsync(int id);
        Task DeleteFileAsync(int actionId, string fileName);
    }
}
