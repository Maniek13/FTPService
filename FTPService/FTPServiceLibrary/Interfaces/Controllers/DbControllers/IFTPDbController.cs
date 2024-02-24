using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace FTPServiceLibrary.Interfaces.DbControllers
{
    public interface IFTPDbController
    {
        Task<IConfigurationDbModel> SetFTPConfigurationAsync(IConfigurationDbModel cfg);
        Task<IConfigurationDbModel> EditFTPConfigurationAsync(IConfigurationDbModel cfg);
        Task RemoveFTPConfigurationAsync(int serviceId);
        Task<IServiceActionDbModel> AddActionFolderAsync(IServiceActionDbModel action);
        Task<IServiceActionDbModel> EditActionFolderAsync(IServiceActionDbModel action);
        Task RemoveActionFolderAsync(string actionName);
    }
}
