using AutoMapper;
using FTPServiceLibrary.Interfaces.Models;
using FTPServiceLibrary.Models;
using FTPServiceLibrary.Models.DbModels;

namespace FTPServiceLibrary.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FTPConfigurationModel, FTPConfigurationDbModel>().ReverseMap();
            CreateMap<ServiceActionModel, ServiceActionDbModel>().ReverseMap();
        }
    }
}
