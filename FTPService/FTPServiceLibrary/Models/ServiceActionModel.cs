using FTPServiceLibrary.Interfaces.Models;

namespace FTPServiceLibrary.Models
{
    public record ServiceActionModel : IServiceActionModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ActionName { get; set; }
        public string Path { get; set; }
    }
}
