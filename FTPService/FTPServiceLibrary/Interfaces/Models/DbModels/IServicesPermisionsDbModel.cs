using FTPServiceLibrary.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Interfaces.Models.DbModels
{
    public interface IServicesPermisionsDbModel
    {
        [Key]
        int Id { get; set; }
        [Required]
        string ServiceName { get; set; }
        FTPConfigurationDbModel Configuration { get; set; }
        ICollection<ServiceActionDbModel> ServicesActions { get; set; }
    }
}
