using FTPServiceLibrary.Interfaces.Configuration;

namespace FTPServiceLibrary.Models
{
    public class AppConfig : IAppConfig
    {
        public static string ConnectionString { get; set; }
        public static string ConnectionStringRO { get; set; }
        public static string SigningKey { get; set; }
    }
}
