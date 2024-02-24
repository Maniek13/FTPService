using Azure.Core;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPServiceLibrary.Helpers
{
    public class FTPHelper
    {
        public static async Task SendFile(IFTPConfiguration cfg, string path, IFormFile files)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{cfg.Url}{path}");
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential()
                {
                    UserName = cfg.Name, Password = cfg.Password, Domain = cfg.Damain
                };

                var temp = files.OpenReadStream();
                byte[] byteArray;

                using (MemoryStream ms = new())
                {
                    ms.Position = 0;
                    temp.CopyTo(ms);
                    byteArray = ms.ToArray();
                }
                using (MemoryStream fileStream = new ())
                {
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Seek(0, SeekOrigin.Begin);

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        await fileStream.CopyToAsync(requestStream);
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            if (response.StatusCode != FtpStatusCode.CommandOK)
                                throw new Exception("Plik nie został wstawiony");
                        }
                    }
                }
                
            
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
