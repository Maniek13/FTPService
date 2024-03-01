using FluentFTP;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace FTPServiceLibrary.Helpers
{
    public class FTPHelper
    {
        object lockFtp;
        // path: /serviceName/actionName
        public static async Task SendFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, IFormFile file)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + serviceName + "\\" + actionPath;
            string fullPath = tempPath + "\\" + file.FileName;
            try
            {
                var token = new CancellationToken();
                using var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                Directory.CreateDirectory(tempPath);

                using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                await ftp.UploadFile(fullPath, serviceName + "//" + actionPath + "//" + file.FileName, FtpRemoteExists.Overwrite, true, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                System.IO.File.Delete(fullPath);
            }
        }
        public static async Task<byte[]> GetFile(IFTPConfigurationModel cfg, string serviceName, string actionPath, string fileName)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + serviceName + "\\" + actionPath;
            string fullPath = tempPath + "\\" + fileName;

            try
            {
                var token = new CancellationToken();



                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFile(fullPath, serviceName + "//" + actionPath + "//" + fileName, token: token);
                }


                var file = File.ReadAllBytes(fullPath);
                File.Delete(fullPath);

                return file;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public static async Task<string> GetPathToZipArchiweWithActionDirectoryFiles(IFTPConfigurationModel cfg, string serviceName, string actionPath, string actionName, string uniqueId)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory;
            string zipFileDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "\\zip\\" + uniqueId;

            Directory.CreateDirectory(zipFileDirectoryPath);

            string zipTempFilePath = Path.Combine(zipFileDirectoryPath, actionName + ".zip");
            string fullPath = tempPath + "\\" + uniqueId;

            try
            {
                var token = new CancellationToken();



                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadDirectory(fullPath, serviceName + "//" + actionPath, token: token);
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
                await ftp.DeleteFile(serviceName + "//" + actionPath + "//" + fileName, token: token);
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
                await ftp.DeleteDirectory(serviceName + "//" + actionPath, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
