using FTPServiceLibrary.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Interfaces.Models.DbModels
{
    public interface IFTPConfigurationDbModel
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
        int Port { get; init; }
        ServicesPermisionsDbModel ServicesPermisions { get; set; }
    }
}
