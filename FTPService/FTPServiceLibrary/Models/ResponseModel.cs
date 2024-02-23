using FTPServiceLibrary.Interfaces.Models;

namespace FTPServiceLibrary.Models
{
    public struct ResponseModel<T> : IResponseModel<T>
    {
        public T Data { get; init; }
        public string message { get; init; }
    }
}
