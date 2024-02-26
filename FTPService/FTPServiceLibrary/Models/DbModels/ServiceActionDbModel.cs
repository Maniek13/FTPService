using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public record ServiceActionDbModel : IServiceActionDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public string ActionName { get; set; }
        [Required]
        public string Path { get; set; }
        public ServicesPermisionsDbModel ServicesPermisions { get; set; }

        public ICollection<FilesDbModel> Files { get; set; }
    }
}
