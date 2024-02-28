using Configuration.Data;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    public class FTPDbController() : IFTPDbController
    {
        public async Task<IFTPConfigurationDbModel> SetFTPConfigurationAsync(IFTPConfigurationDbModel cfg)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                await _context.FtpConfigurations.AddAsync((FTPConfigurationDbModel)cfg);
                await _context.SaveChangesAsync();
                return cfg;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<IFTPConfigurationDbModel> EditFTPConfigurationAsync(IFTPConfigurationDbModel cfg)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                _context.FtpConfigurations.Update((FTPConfigurationDbModel)cfg);
                await _context.SaveChangesAsync();
                return cfg;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RemoveFTPConfigurationAsync(int serviceId)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                var temp = _context.FtpConfigurations.Where(el => el.ServiceId == serviceId).FirstOrDefault();
                _context.FtpConfigurations.Remove(temp);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IServiceActionDbModel> AddActionFolderAsync(IServiceActionDbModel action)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                await _context.FtpServicesActions.AddAsync((ServiceActionDbModel)action);
                await _context.SaveChangesAsync();
                return action;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IServiceActionDbModel> EditActionFolderAsync(IServiceActionDbModel action)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                _context.FtpServicesActions.Update((ServiceActionDbModel)action);
                await _context.SaveChangesAsync();
                return action;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RemoveActionFolderAsync(string actionName)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                var temp = _context.FtpServicesActions.Where(el => el.ActionName == actionName).FirstOrDefault();
                _context.FtpServicesActions.Remove(temp);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task AddFile(IFilesDbModel file)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                await _context.FtpFiles.AddAsync((FilesDbModel)file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteFile(int id)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                var file = _context.FtpFiles.Where(el => el.Id == id).FirstOrDefault();
                _context.FtpFiles.Remove(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteFile(int actionId, string fileName)
        {
            try
            {
                using FTPServiceContext _context = new FTPServiceContext(AppConfig.ConnectionString);
                var file = _context.FtpFiles.Where(el => el.ServiceActionId == actionId && el.Name == fileName).FirstOrDefault();
                _context.FtpFiles.Remove(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
