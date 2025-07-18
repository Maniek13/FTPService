﻿using FTPServiceLibrary.Interfaces.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace FTPServiceLibrary.Models.DbModels
{
    public record FTPConfigurationDbModel : IFTPConfigurationDbModel
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
        public ServicesPermisionsDbModel ServicesPermisions { get; set; }
    }
}
