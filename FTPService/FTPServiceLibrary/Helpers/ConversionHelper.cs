using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Interfaces.Models.DbModels;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServiceLibrary.Helpers
{
    public class ConversionHelper
    {
        public static FTPConfiguration ConveretToFtpConfuguration(IConfigurationDbModel cfg)
        {
            return new FTPConfiguration()
            {
                Id = cfg.Id,
                ServiceId = cfg.ServiceId,
                Name = cfg.Name,
                Url = cfg.Url,
                Login = cfg.Login,
                Password = cfg.Password,
                Port = cfg.Port,
                Damain = cfg.Damain,
            };
        }

        public static ConfigurationDbModel ConveretToConfigurationDbModel(IFTPConfiguration cfg)
        {
            return new ConfigurationDbModel()
            {
                Id = cfg.Id,
                ServiceId = cfg.ServiceId,
                Name = cfg.Name,
                Url = cfg.Url,
                Login = cfg.Login,
                Password = cfg.Password,
                Port = cfg.Port,
                Damain = cfg.Damain,
            };
        }

        public static ServicesAction ConvertToServicesAction(ServiceActionDbModel action)
        {
            return new ServicesAction()
            {
                Id = action.Id,
                ServiceId= action.ServiceId,
                ActionName = action.ActionName,
                Path = action.Path
            };
        }

        public static ServiceActionDbModel ConvertToServiceActionDbModel(ServicesAction action)
        {
            return new ServiceActionDbModel()
            {
                Id = action.Id,
                ServiceId = action.ServiceId,
                ActionName = action.ActionName,
                Path = action.Path
            };
        }
    }
}
