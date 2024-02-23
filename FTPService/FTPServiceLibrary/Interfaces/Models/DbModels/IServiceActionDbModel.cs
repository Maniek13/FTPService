using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public interface IServiceActionDbModel
    {
        [Key]
        int Id { get; set; }
        [Required]
        int ServiceId { get; set; }
        [Required]
        string ActionName { get; set; }
        [Required]
        string Path { get; set; }
        ServicesPermisionsDbModel ServicesPermisions { get; set; }

        ICollection<FilesDbModel> Files { get; set; }
    }
}
