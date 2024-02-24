using FTPServiceLibrary.Interfaces.Models;

namespace FTPServiceLibrary.Models
{
    public readonly struct ResponseModel<T> : IResponseModel<T>
    {
        public T Data { get; init; }
        public string Message { get; init; }
    }
}
