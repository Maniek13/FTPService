using FTPServiceLibrary.Interfaces.Models;
using Microsoft.IdentityModel.Tokens;

namespace FTPServiceLibrary.Helpers
{
    public class ValidationHelper
    {
        public static void ValidateFTPConfigurationModel(IFTPConfigurationModel cfg)
        {
            try
            {
                if (cfg == null)
                    throw new ArgumentNullException("Nie przekazano konfiguracji");
                if (string.IsNullOrWhiteSpace(cfg.Name))
                    throw new ArgumentOutOfRangeException("Brak nazwy konfiguracji");
                if (string.IsNullOrWhiteSpace(cfg.Url))
                    throw new ArgumentOutOfRangeException("Brak adresu serwera ftp");
                if (cfg.Port == 0)
                    throw new ArgumentOutOfRangeException("Port nie może być zerem");
                if (string.IsNullOrWhiteSpace(cfg.Login))
                    throw new ArgumentOutOfRangeException("Login nie moze byc pusty");
                if (string.IsNullOrWhiteSpace(cfg.Login))
                    throw new ArgumentOutOfRangeException("Hasło nie moze byc puste");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message, ex);
            }
        }

        public static void ValidateServiceActionModel(IServiceActionModel action)
        {
            try
            {
                if (action == null)
                    throw new ArgumentNullException("Nie przekazano objektu akcji");
                if (string.IsNullOrWhiteSpace(action.ActionName))
                    throw new ArgumentOutOfRangeException("NAzwa akcji nie może być pusta");
                if (string.IsNullOrWhiteSpace(action.Path))
                    throw new ArgumentOutOfRangeException("Path do folderu akcji nie moze być pusty");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message, ex);
            }
        }

        public static void ValidateServiceName(string serviceName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serviceName))
                    throw new ArgumentOutOfRangeException("Prosze podac nazwę serwisu");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message, ex);
            }
        }

        public static void ValidateActionName(string actionName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionName))
                    throw new ArgumentOutOfRangeException("Prosze podac nazwę akcji");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message, ex);
            }
        }
        public static void ValidateFileName(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentOutOfRangeException("Prosze podac nazwę pliku");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message, ex);
            }
        }
    }
}
