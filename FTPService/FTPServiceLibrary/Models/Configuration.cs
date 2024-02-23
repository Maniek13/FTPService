namespace FTPServiceLibrary.Interfaces.Models
{
    internal interface Configuration
    {
        public int Id { get; init; }
        public int ServiceId { get; set; }
        public string Name { get; init; }
        public string Url { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
