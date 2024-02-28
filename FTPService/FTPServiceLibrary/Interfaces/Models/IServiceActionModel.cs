using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IServicesAction
    {
        int Id { get; set; }
        int ServiceId { get; set; }
        string ActionName { get; set; }
        string Path { get; set; }
    }
}
