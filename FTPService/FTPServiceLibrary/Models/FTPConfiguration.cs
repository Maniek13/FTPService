namespace FTPServiceLibrary.Interfaces.Models
{
    public record FTPConfiguration : IFTPConfiguration
    {
        public int Id { get; init; }
        public int ServiceId { get; set; }
        public string Name { get; init; }
        public string Url { get; init; }
        public string Port { get; init; }
        public string Damain {  get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
