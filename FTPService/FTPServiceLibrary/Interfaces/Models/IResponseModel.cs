namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IResponseModel<T>
    {
        T Data { get; init; }
        string Message { get; init; }
    }
}
