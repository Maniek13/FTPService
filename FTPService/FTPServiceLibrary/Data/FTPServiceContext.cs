namespace FTPServiceLibrary.Data
{
    public class FTPServiceContext : FTPServiceContextBase
    {
        public FTPServiceContext(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

    }
}
