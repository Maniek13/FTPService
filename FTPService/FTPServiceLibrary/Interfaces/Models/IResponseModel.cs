using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IResponseModel<T>
    {
        T Data { get; init; }
        string message {  get; init; }
    }
}
