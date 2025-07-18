﻿namespace FTPServiceLibrary.Interfaces.Models
{
    public record FTPConfigurationModel : IFTPConfigurationModel
    {
        public int Id { get; init; }
        public int ServiceId { get; set; }
        public string Name { get; init; }
        public string Url { get; init; }
        public int Port { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
