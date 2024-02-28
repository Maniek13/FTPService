using AutoMapper;
using Configuration.Controllers.DbControllers;
using FTPServiceLibrary.Helpers;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace FTPServiceTests.Tests.Configuration.Controllers.WebControllers
{
    public class ConfigurationWebControllerTests
    {
        FTPRODbController _ftpRODbController;
        FTPDbController _ftpDbController;
        HttpContext _httpContext;
        ConfigurationWebController _controller;
        IMapper _mapper;
        ILogger _logger;

        public ConfigurationWebControllerTests()
        {
            Helper.SetConnectionStrings();
            _ftpRODbController = new();
            _ftpDbController = new();
            _httpContext = new DefaultHttpContext();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());

            _mapper = mapperConfig.CreateMapper();
            _logger = loggerFactory.CreateLogger<Program>();

            _controller = new ConfigurationWebController(_mapper, _logger, _ftpRODbController, _ftpDbController);
        }
        [Fact]
        public async Task ConfigurationTests()
        {
            try
            {
                FTPConfigurationModel cfgToAdd = new()
                {
                    Name = "test",
                    Url = "nowy",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };
                _ = await _controller.AddConfigurationAsync("test", cfgToAdd, _httpContext);
                var cfgAdded = _controller.GetConfiguration("test", _httpContext);
                if (cfgAdded.Data == null ||
                    cfgAdded.Data.Id == 0 ||
                    cfgAdded.Data.ServiceId == 0 ||
                    cfgAdded.Data.Url != "nowy" ||
                    cfgAdded.Data.Port != 245 ||
                    cfgAdded.Data.Name != "test" ||
                    cfgAdded.Data.Login != "test" ||
                    cfgAdded.Data.Password != "12345678")
                    Assert.Fail("nie dodano");



                FTPConfigurationModel cfgToAdd2 = new()
                {
                    Name = "test",
                    Url = "nowy",
                    Port = 245,
                    Login = "test",
                    Password = "12345678"
                };
                var secondCfg = await _controller.AddConfigurationAsync("test", cfgToAdd2, _httpContext);
                if (secondCfg.Data != null)
                    Assert.Fail("Dodano kolejną konfigurację");



                FTPConfigurationModel cfgToEdit = new()
                {
                    Id = cfgAdded.Data.Id,
                    ServiceId = cfgAdded.Data.ServiceId,
                    Name = "edited",
                    Url = "edited",
                    Port = 1234,
                    Login = "edited",
                    Password = "edited"
                };
                _ = await _controller.EditConfigurationAsync("test", cfgToEdit, _httpContext);
                var cfgEdited = _controller.GetConfiguration("test", _httpContext);
                if (cfgEdited.Data == null ||
                    cfgEdited.Data.Id != cfgAdded.Data.Id ||
                    cfgEdited.Data.ServiceId == cfgAdded.Data.Id ||
                    cfgEdited.Data.Url != "edited" ||
                    cfgEdited.Data.Port != 1234 ||
                    cfgEdited.Data.Name != "edited" ||
                    cfgEdited.Data.Login != "edited" ||
                    cfgEdited.Data.Password != "edited")
                    Assert.Fail("nie edytowano");


                _ = await _controller.DeleteConfigurationAsync("test", _httpContext);
                var cfgDeleted = _controller.GetConfiguration("test", _httpContext);

                if (cfgDeleted.Data != null)
                    Assert.Fail("nie usunięto");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                var toDel = _controller.GetConfiguration("test", _httpContext);
                if (toDel.Data != null)
                    _ = await _controller.DeleteConfigurationAsync("test", _httpContext);
            }
        }
        [Fact]
        public async Task ActionFolderTests()
        {
            try
            {
                ServiceActionModel actionModel = new()
                {
                    ActionName = "test",
                    Path = "test",
                };
                await _controller.AddActionFolderAsync("test", actionModel, _httpContext);
                var actionAdd = _controller.GetActionsFolders("test", _httpContext);
                if (actionAdd.Data == null ||
                    actionAdd.Data[0].Id == 0 ||
                    actionAdd.Data[0].ActionName != "test" ||
                    actionAdd.Data[0].Path != "test" ||
                    actionAdd.Data.Count != 1)
                    Assert.Fail("nie dodano");




                ServiceActionModel actionModelEdited = new()
                {
                    Id = actionAdd.Data[0].Id,
                    ServiceId = actionAdd.Data[0].ServiceId,
                    ActionName = "edit",
                    Path = "edit",
                };
                await _controller.EditeActionFolderAsync("test", actionModelEdited, _httpContext);
                var actionEdited = _controller.GetActionsFolders("test", _httpContext);
                if (actionEdited.Data == null ||
                    actionEdited.Data[0].Id != actionAdd.Data[0].Id ||
                    actionEdited.Data[0].ActionName != "edit" ||
                    actionEdited.Data[0].Path != "edit" ||
                    actionEdited.Data.Count != 1)
                    Assert.Fail("nie edytowano");



                ServiceActionModel actionModel2 = new()
                {
                    ActionName = "test2",
                    Path = "test2",
                };
                await _controller.AddActionFolderAsync("test", actionModel2, _httpContext);
                var actionAdded2 = _controller.GetActionsFolders("test", _httpContext);
                if (actionAdded2.Data == null ||
                    actionAdded2.Data[1].Id == 0 ||
                    actionAdded2.Data[1].ActionName != "test2" ||
                    actionAdded2.Data[1].Path != "test2" ||
                    actionAdded2.Data.Count != 2)
                    Assert.Fail("Nie dodano kolejnego");


                ServiceActionModel actionModel3 = new()
                {
                    ActionName = "test2",
                    Path = "test2",
                };
                await _controller.AddActionFolderAsync("test", actionModel3, _httpContext);
                var actionAdded3 = _controller.GetActionsFolders("test", _httpContext);


                if (actionAdded3.Data == null ||
                    actionAdded3.Data.Count != 2)
                    Assert.Fail("Dodano ten sam");





                await _controller.DeleteActionFolderAsync("test", "edit", _httpContext);
                await _controller.DeleteActionFolderAsync("test", "test2", _httpContext);


                var actionsDeleted = _controller.GetActionsFolders("test", _httpContext);

                if (actionsDeleted.Data == null || actionsDeleted.Data.Count != 0)
                    Assert.Fail("Nie usunięto");


            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                var actionsDeleted = _controller.GetActionsFolders("test", _httpContext);

                for (int i = 0; i < actionsDeleted.Data.Count; i++)
                    await _controller.DeleteActionFolderAsync("test", actionsDeleted.Data[i].ActionName, _httpContext);
            }
        }
    }
}
