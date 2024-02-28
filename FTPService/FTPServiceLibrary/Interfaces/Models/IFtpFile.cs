namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IFtpFile
    {
        string Name { get; set; }
        byte[] Data { get; set; }

    }
}
