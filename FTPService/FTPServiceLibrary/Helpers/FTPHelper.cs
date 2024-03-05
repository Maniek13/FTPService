using FluentFTP;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

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

                GarbageCollectionOfTempDirPath(ref fullDirPath, tempDirPath);

                return zipTempFilePath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(fullDirPath) && fullDirPath.StartsWith(tempDirPath))
                    GarbageCollectionOfTempDirPath(ref fullDirPath, tempDirPath);
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

        private static void GarbageCollectionOfTempDirPath(ref string actualFolderPath, string tempDirPath)
        {
            try
            {
                if (!actualFolderPath.StartsWith(tempDirPath))
                    throw new Exception($"Aktualna ścieżka ({actualFolderPath}), musi zaczynać się od tymczasowej ścieżki: ({tempDirPath})");

                if (string.IsNullOrWhiteSpace(actualFolderPath) || string.IsNullOrWhiteSpace(tempDirPath))
                    throw new Exception($"Nazwy ścieżek nie mogą być puste. Aktualna ścieżka: {actualFolderPath}, ścieżka tymczasowa: {tempDirPath}");

                var files = Directory.GetFiles(actualFolderPath);
                if (files != null)
                    for (int i = 0; i < files.Length; ++i)
                        File.Delete(files[i]);

                var subDirectorys = Directory.GetDirectories(actualFolderPath);

                if(subDirectorys.Count() != 0)
                {
                    actualFolderPath = subDirectorys[0];
                    GarbageCollectionOfTempDirPath(ref actualFolderPath, tempDirPath);
                }

                if (tempDirPath == actualFolderPath)
                {
                    Directory.Delete(tempDirPath);
                    actualFolderPath = string.Empty;
                }

                else if (!string.IsNullOrWhiteSpace(actualFolderPath))
                {
                    Directory.Delete(actualFolderPath);
                    var parent = Directory.GetParent(actualFolderPath) ?? throw new Exception("Usuwanie śmieci, brak podkatalogu. Coś poszło nie tak, spróbuj ponownie.");

                    if (parent != null)
                        actualFolderPath = parent.FullName;

                    GarbageCollectionOfTempDirPath(ref actualFolderPath, tempDirPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
