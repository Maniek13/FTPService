using FluentFTP;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace FTPServiceLibrary.Helpers
{
    public class FTPHelper
    {
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        public static async Task SendFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, IFormFile file)
        {
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, serviceName, actionPath);
            string fullPath = Path.Combine(tempPath, file.FileName);
            try
            {
                var token = new CancellationToken();
                using var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                Directory.CreateDirectory(tempPath);

                _semaphore.Wait();
                using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                await ftp.UploadFile(fullPath, Path.Combine(serviceName, actionPath, file.FileName), FtpRemoteExists.Overwrite, true, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fullPath);
                _semaphore.Release();
            }
        }
        public static async Task<byte[]> GetFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, string fileName)
        {
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, serviceName, actionPath);
            string fullPath = Path.Combine(tempPath, fileName);

            try
            {
                var token = new CancellationToken();
                byte[] file;

                _semaphore.Wait();

                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFile(fullPath, Path.Combine(serviceName, actionPath, fileName), token: token);
                    file = File.ReadAllBytes(fullPath);
                }

                return file;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fullPath);
                _semaphore.Release();
            }
        }

        public static async Task<string> CreateZipArchiveWithActionDirectoryFiles(IFTPConfigurationModel cfg, string serviceName, string actionPath, string actionName, string uniqueId)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory;
            string zipFileDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zip", uniqueId);

            Directory.CreateDirectory(zipFileDirectoryPath);

            string zipTempFilePath = Path.Combine(zipFileDirectoryPath, actionName + ".zip");
            string fullPath = Path.Combine(tempPath, uniqueId);

            try
            {
                var token = new CancellationToken();
                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadDirectory(fullPath, Path.Combine(serviceName, actionPath), token: token);
                }

                ZipFile.CreateFromDirectory(fullPath, zipTempFilePath);

                var filesDir = Path.Combine(fullPath, serviceName, actionName);
                var files = Directory.GetFiles(filesDir);

                for (int i = 0; i < files.Length; ++i)
                    File.Delete(files[i]);

                Directory.Delete(filesDir);
                Directory.Delete(Path.Combine(fullPath, serviceName));
                Directory.Delete(fullPath);

                return zipTempFilePath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
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
