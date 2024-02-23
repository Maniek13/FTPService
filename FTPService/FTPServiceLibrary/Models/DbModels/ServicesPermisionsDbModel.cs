using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public class ServicesPermisionsDbModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ServiceName { get; set; }
        public ConfigurationDbModel Configuration { get; set; }
        public ICollection<ServiceActionDbModel> ServicesActions { get; set; }
    }
}
