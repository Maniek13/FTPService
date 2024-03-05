using FTPServiceLibrary.Models.DbModels;
using Microsoft.EntityFrameworkCore;
namespace FTPServiceLibrary.Interfaces.Data
{
    public interface IFTPServiceContextBase
    {
        virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        virtual DbSet<ServicesPermisionsDbModel> ServicesPermisions => throw new NotImplementedException();
        virtual DbSet<FileDbModel> Files => throw new NotImplementedException();
        virtual DbSet<ServiceActionDbModel> ServicesActions => throw new NotImplementedException();
        virtual DbSet<FTPConfigurationDbModel> Configurations => throw new NotImplementedException();
    }
}
