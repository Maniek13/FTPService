using FTPServiceLibrary.Interfaces.Models;

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
                if (String.IsNullOrWhiteSpace(cfg.Name))
                    throw new ArgumentOutOfRangeException("Brak nazwy konfiguracji");
                if (String.IsNullOrWhiteSpace(cfg.Url))
                    throw new ArgumentOutOfRangeException("Brak adresu serwera ftp");
                if (cfg.Port == 0)
                    throw new ArgumentOutOfRangeException("Port nie może być zerem");
                if (String.IsNullOrWhiteSpace(cfg.Login))
                    throw new ArgumentOutOfRangeException("Login nie moze byc pusty");
                if (String.IsNullOrWhiteSpace(cfg.Login))
                    throw new ArgumentOutOfRangeException("Hasło nie moze byc puste");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void ValidateServiceActionModel(IServiceActionModel action)
        {
            try
            {
                if (action == null)
                    throw new ArgumentNullException("Nie przekazano objektu akcji");
                if (String.IsNullOrWhiteSpace(action.ActionName))
                    throw new ArgumentOutOfRangeException("NAzwa akcji nie może być pusta");
                if (String.IsNullOrWhiteSpace(action.Path))
                    throw new ArgumentOutOfRangeException("Path do folderu akcji nie moze być pusty");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void ValidateServiceName(string serviceName)
        {
            try
            {
                if (serviceName == "")
                    throw new Exception("Prosze podac nazwę serwisu");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static void ValidateActionName(string actionName)
        {
            try
            {
                if (actionName == "")
                    throw new Exception("Prosze podac nazwę akcji");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public static void ValidateFileName(string fileName)
        {
            try
            {
                if (fileName == "")
                    throw new Exception("Prosze podac nazwę pliku");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
