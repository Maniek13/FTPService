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
                    Url = "127.0.0.1",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };

                using var stream = File.OpenRead("C:\\Users\\mani3\\OneDrive\\Pulpit\\Praca\\1234.jpg");
                
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };

                await FTPHelper.SendFile(cfg, "test", "akcja", file);
                
                
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }

        
        [Fact]
        public async Task GetFileTest()
        {
            try
            {
                FTPConfiguration cfg = new()
                {
                    Url = "127.0.0.1",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };

                var f = await FTPHelper.GetFile(cfg, "test", "akcja", "1234.jpg");
                    if (f.FileName != "1234.jpg")
                        Assert.Fail("zła nazwa");
    

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }


        [Fact]
        public async Task DeleteFile()
        {
            try
            {
                FTPConfiguration cfg = new()
                {
                    Url = "127.0.0.1",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };


                await FTPHelper.DeleteFile(cfg, "test", "akcja", "1234.jpg");
                Assert.ThrowsAsync<Exception>(async () => await FTPHelper.GetFile(cfg, "test", "akcja", "1234.jpg"));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [Fact]
        public async Task DeleteAllActionFiles()
        {
            try
            {
                FTPConfiguration cfg = new()
                {
                    Url = "127.0.0.1",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };


                await FTPHelper.DeleteAllFiles(cfg, "test", "akcja");
                Assert.ThrowsAsync<Exception>(async () => await FTPHelper.GetFile(cfg, "test", "akcja", "1234.jpg"));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
