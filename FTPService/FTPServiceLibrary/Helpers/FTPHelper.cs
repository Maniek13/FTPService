using FluentFTP;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Net.WebSockets;

namespace FTPServiceLibrary.Helpers
{
    public class FTPHelper
    {
        public static async Task SendFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, IFormFile file)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string tempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uniqueId);
            string fileFullPath = Path.Combine(tempDirPath, file.FileName);
            try
            {
                var token = new CancellationToken();
                using var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                Directory.CreateDirectory(tempDirPath);

                using (Stream fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                await ftp.UploadFile(fileFullPath, Path.Combine(serviceName, actionPath, file.FileName), FtpRemoteExists.Overwrite, true, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fileFullPath);
                Directory.Delete(tempDirPath);
            }
        }
        public static async Task<byte[]> GetFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, string fileName)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string tempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uniqueId);
            string fileFullPath = Path.Combine(tempDirPath, fileName);

            try
            {
                var token = new CancellationToken();
                byte[] file;

                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFile(fileFullPath, Path.Combine(serviceName, actionPath, fileName), token: token);
                    file = File.ReadAllBytes(fileFullPath);
                }

                return file;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fileFullPath);
                Directory.Delete(tempDirPath);
            }
        }

        public static async Task<string> CreateZipArchiveWithActionDirectoryFiles(IFTPConfigurationModel cfg, string serviceName, string actionPath, string actionName)
        {
            string uniqueId = Guid.NewGuid().ToString();
            string zipFileDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zip", uniqueId); 
            string zipTempFilePath = Path.Combine(zipFileDirectoryPath, actionName + ".zip");
            Directory.CreateDirectory(zipFileDirectoryPath);

            string tempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uniqueId);
            string fullDirPath = Path.Combine(tempDirPath, serviceName, actionPath);
            try
            {
                var token = new CancellationToken();
                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadDirectory(tempDirPath, Path.Combine(serviceName, actionPath), token: token);
                }
                ZipFile.CreateFromDirectory(fullDirPath, zipTempFilePath);

                DeletingFilesAndParentFolders(ref fullDirPath, tempDirPath);

                return zipTempFilePath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                if(!string.IsNullOrEmpty(fullDirPath))
                    DeletingFilesAndParentFolders(ref fullDirPath, tempDirPath);
            }
        }
        private static void DeletingFilesAndParentFolders(ref string fullDirPath, string tempDirPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fullDirPath) || string.IsNullOrWhiteSpace(tempDirPath))
                    throw new Exception($"Nazwy ścieżek nie mogą być puste. Pełna ścieżka: {fullDirPath}, ścieżka tymczasowa: {tempDirPath}");

                if(tempDirPath == fullDirPath)
                {
                    Directory.Delete(tempDirPath);
                    fullDirPath = string.Empty;
                    return;
                }

                var files = Directory.GetFiles(fullDirPath);

                if (files != null)
                    for (int i = 0; i < files.Length; ++i)
                        File.Delete(files[i]);
                Directory.Delete(fullDirPath);

                fullDirPath = Directory.GetParent(fullDirPath).FullName;

                DeletingFilesAndParentFolders(ref fullDirPath, tempDirPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public static async Task DeleteFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, string fileName)
        {
            try
            {
                var token = new CancellationToken();
                using AsyncFtpClient ftp = new(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                await ftp.DeleteFile(Path.Combine(serviceName, actionPath, fileName), token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public static async Task DeleteDirectory(IFTPConfigurationModel cfg, string serviceName, string actionPath)
        {
            try
            {
                var token = new CancellationToken();
                using var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                await ftp.DeleteDirectory(Path.Combine(serviceName, actionPath), token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

    }
}
