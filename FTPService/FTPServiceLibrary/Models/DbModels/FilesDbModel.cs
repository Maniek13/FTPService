using FTPServiceLibrary.Interfaces.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public class FilesDbModel : IFilesDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ServiceActionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
        public ServiceActionDbModel ServiceAction { get; set; }
    }
}
