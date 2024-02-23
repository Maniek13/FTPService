namespace FTPServiceLibrary.Interfaces.Models
{
    internal interface IConfiguration
    {
        int Id { get; init; }
        int ServiceId { get; set; }
        string Name { get; init; }
        string Url { get; init; }
        string Login { get; init; }
        string Password { get; init; }
    }
}
