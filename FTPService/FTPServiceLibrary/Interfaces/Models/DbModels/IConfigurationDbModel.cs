using FTPServiceLibrary.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Interfaces.Models.DbModels
{
    public interface IConfigurationDbModel
    {
        [Key]
        int Id { get; set; }
        [Required]
        int ServiceId { get; set; }
        [Required]
        string Name { get; set; }
        [Required]
        string Url { get; set; }
        [Required]
        string Login { get; set; }
        [Required]
        string Password { get; set; }
        [Required]
        string Port { get; init; }
        [Required]
        string Damain { get; init; }
        ServicesPermisionsDbModel ServicesPermisions { get; set; }
    }
}
