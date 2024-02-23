using FTPServiceLibrary.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Interfaces.Models.DbModels
{
    public interface IFilesDbModel
    {
        [Key]
        int Id { get; set; }
        [Required]
        int ServiceActionId { get; set; }
        [Required]
        string Name { get; set; }
        [Required]
        string Path { get; set; }
        ServiceActionDbModel ServiceAction { get; set; }
    }
}
