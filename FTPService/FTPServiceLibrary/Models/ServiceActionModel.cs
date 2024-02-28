using FTPServiceLibrary.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServiceLibrary.Models
{
    public record ServicesAction : IServicesAction
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ActionName { get; set; }
        public string Path { get; set; }
    }
}
