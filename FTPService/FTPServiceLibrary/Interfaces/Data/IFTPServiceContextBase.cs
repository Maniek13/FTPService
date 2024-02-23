using FTPServiceLibrary.Models.DbModels;
using Microsoft.EntityFrameworkCore;
namespace FTPServiceLibrary.Interfaces.Data
{
    public interface IFTPServiceContextBase
    {
        virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        virtual DbSet<ServicesPermisionsDbModel> ServicesPermisions => throw new NotImplementedException();
        virtual DbSet<FilesDbModel> Files => throw new NotImplementedException();
        virtual DbSet<ServiceActionDbModel> ServicesActions => throw new NotImplementedException();
        virtual DbSet<ConfigurationDbModel> Configurations => throw new NotImplementedException();
    }
}
