using Configuration.Data;
using FTPServiceLibrary.Interfaces.Data;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    public class FTPDbController(IFTPServiceContextBase dbContext) : IFTPDbController
    {
        readonly IFTPServiceContextBase _context = dbContext;
        
        public async Task<IConfigurationDbModel> SetFTPConfigurationAsync(IConfigurationDbModel cfg)
        {
            try
            {
                await _context.Configurations.AddAsync((ConfigurationDbModel)cfg);
                await _context.SaveChangesAsync();
                return cfg;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<IConfigurationDbModel> EditFTPConfigurationAsync(IConfigurationDbModel cfg)
        {
            try
            {
                _context.Configurations.Update((ConfigurationDbModel)cfg);
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
                var temp = _context.Configurations.Where(el => el.ServiceId == serviceId).FirstOrDefault();
                _context.Configurations.Remove(temp);
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
                await _context.ServicesActions.AddAsync((ServiceActionDbModel)action);
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
                _context.ServicesActions.Update((ServiceActionDbModel)action);
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
                var temp = _context.ServicesActions.Where(el => el.ActionName == actionName).FirstOrDefault();
                _context.ServicesActions.Remove(temp);
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
                await _context.Files.AddAsync((FilesDbModel)file);
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
                var file = _context.Files.Where(el => el.Id == id).FirstOrDefault();
                _context.Files.Remove(file);
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
                var file = _context.Files.Where(el => el.ServiceActionId == actionId && el.Name == fileName).FirstOrDefault();
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
