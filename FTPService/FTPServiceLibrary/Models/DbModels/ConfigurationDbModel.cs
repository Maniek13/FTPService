using FTPServiceLibrary.Interfaces.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public class ConfigurationDbModel : IConfigurationDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int Port { get; init; }
        [Required]
        public string Damain { get; init; }
        public ServicesPermisionsDbModel ServicesPermisions { get; set; }
    }
}
