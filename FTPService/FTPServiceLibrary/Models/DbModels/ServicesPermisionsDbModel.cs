using FTPServiceLibrary.Interfaces.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public record ServicesPermisionsDbModel : IServicesPermisionsDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ServiceName { get; set; }
        public FTPConfigurationDbModel Configuration { get; set; }
        public ICollection<ServiceActionDbModel> ServicesActions { get; set; }

    }
}
