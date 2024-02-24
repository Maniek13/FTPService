using Configuration.Data;
using FTPServiceLibrary.Interfaces.Data;
using FTPServiceLibrary.Interfaces.DbControllers;
using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models.DbModels;

namespace Configuration.Controllers.DbControllers
{
    public class FTPRODbController(IFTPServiceContextBase dbContext) : IFTPRODbController
    {
        private readonly IFTPServiceContextBase _context = dbContext;

        public ServicesPermisionsDbModel GetPermision(string serviceName)
        {
            try
            {
                return _context.ServicesPermisions.Where(el => el.ServiceName == serviceName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ConfigurationDbModel GetFTPConfiguration(int serviceId)
        {
            try
            {
                return _context.Configurations.Where(el => el.ServiceId == serviceId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<ServiceActionDbModel> GetServiceActions(int serviceId)
        {
            try
            {
                return _context.ServicesActions.Where(el => el.ServiceId == serviceId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
