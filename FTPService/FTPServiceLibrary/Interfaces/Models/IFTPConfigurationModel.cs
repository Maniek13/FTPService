﻿namespace FTPServiceLibrary.Interfaces.Models
{
    public interface IFTPConfigurationModel
    {
        int Id { get; init; }
        int ServiceId { get; set; }
        string Name { get; init; }
        string Url { get; init; }
        int Port { get; init; }
        string Login { get; init; }
        string Password { get; init; }
    }
}
