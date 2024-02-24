namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IFTPConfiguration
    {
        int Id { get; init; }
        int ServiceId { get; set; }
        string Name { get; init; }
        string Url { get; init; }
        string Damain { get; init; }
        string Login { get; init; }
        int Port { get; init; }
        string Password { get; init; }
    }
}
