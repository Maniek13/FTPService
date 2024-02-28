namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IServiceActionModel
    {
        int Id { get; set; }
        int ServiceId { get; set; }
        string ActionName { get; set; }
        string Path { get; set; }
    }
}
