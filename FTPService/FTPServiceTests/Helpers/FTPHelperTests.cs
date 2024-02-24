using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.Models;
using Microsoft.AspNetCore.Http;

namespace FTPServiceTests.Helpers
{
    public class FTPHelperTests
    {
        [Fact]
        public async Task SendFileTest()
        {
            try
            {
                FTPConfiguration cfg = new()
                {
                    Url ="",
                    Port = "",
                    Login = "",
                    Password = "@"
                };

                using (var stream = File.OpenRead("C:\\Users\\mani3\\OneDrive\\Pulpit\\Praca\\logo.jpg"))
                {
                    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary()
                    };

                    await FTPHelper.SendFile(cfg, "\\test1234\\logo.jpg", file);
                }
                
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }
    }
}
