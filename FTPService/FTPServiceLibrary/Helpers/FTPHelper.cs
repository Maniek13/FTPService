using FluentFTP;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;

namespace FTPServiceLibrary.Helpers
{
    public class FTPHelper
    {
        // path: /serviceName/actionName
        public static async Task SendFile(IFTPConfiguration cfg, string serviceName, string actionName, IFormFile file)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + serviceName + "\\" + actionName;
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

                await ftp.UploadFile(fullPath, serviceName + "//" + actionName + "//" + file.FileName, FtpRemoteExists.Overwrite, true, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fullPath);
            }
        }
        public static async Task<IFormFile> GetFile(IFTPConfiguration cfg, string serviceName, string actionName, string fileName)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + serviceName + "\\" + actionName;
            string fullPath = tempPath + "\\" + fileName;

            try
            {
                var token = new CancellationToken();

                using (var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFile(fullPath, serviceName + "//" + actionName + "//" + fileName, token: token);
                }


                using var stream = File.OpenRead(fullPath);
                return new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                File.Delete(fullPath);
            }
        }

        public static async Task DeleteFile(IFTPConfiguration cfg, string serviceName, string actionName, string fileName)
        {
            try
            {
                var token = new CancellationToken();
                using AsyncFtpClient ftp = new(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                await ftp.DeleteFile(serviceName + "//" + actionName + "//" + fileName, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public static async Task DeleteAllFiles(IFTPConfiguration cfg, string serviceName, string actionName)
        {
            try
            {
                var token = new CancellationToken();
                using var ftp = new AsyncFtpClient(cfg.Url, cfg.Login, cfg.Password, cfg.Port);
                await ftp.Connect(token);
                await ftp.DeleteDirectory(serviceName + "//" + actionName, token: token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
