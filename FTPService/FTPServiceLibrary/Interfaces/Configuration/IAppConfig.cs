namespace FTPServiceLibrary.Interfaces.Configuration
{
    public interface IAppConfig
    {
        static abstract string ConnectionString { get; set; }
        static abstract string ConnectionStringRO { get; set; }
        static abstract string SigningKey { get; set; }
    }
}
