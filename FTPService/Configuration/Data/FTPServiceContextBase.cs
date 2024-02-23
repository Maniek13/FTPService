using FTPServiceLibrary.Interfaces.Data;
using FTPServiceLibrary.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Data
{
    public class FTPServiceContextBase : DbContext, IFTPServiceContextBase
    {
        internal string ConnectionString { get; init; }

        public FTPServiceContextBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public FTPServiceContextBase()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json");
            var config = configuration.Build();


            ConnectionString = config.GetSection("AppConfig").GetSection("Connection").Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(ConnectionString);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServicesPermisionsDbModel>().HasIndex(u => u.ServiceName).IsUnique();
            modelBuilder.Entity<ServiceActionDbModel>().HasIndex(u => u.ActionName).IsUnique();

            modelBuilder.Entity<ServicesPermisionsDbModel>()
               .HasOne<ConfigurationDbModel>(x => x.Configuration)
               .WithOne(y => y.ServicesPermisions)
               .HasForeignKey<ConfigurationDbModel>(x => x.ServiceId)
               .OnDelete(DeleteBehavior.Cascade);

            //one to many
            modelBuilder.Entity<ServiceActionDbModel>()
             .HasOne<ServicesPermisionsDbModel>(x => x.ServicesPermisions)
             .WithMany(y => y.ServicesActions)
             .HasForeignKey(x => x.ServiceId)
             .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<FilesDbModel>()
             .HasOne<ServiceActionDbModel>(x => x.ServiceAction)
             .WithMany(y => y.Files)
             .HasForeignKey(x => x.ServiceActionId)
             .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<ServicesPermisionsDbModel> ServicesPermisions { get; set; }
        public virtual DbSet<FilesDbModel> Files  {get; set;}
        public virtual DbSet<ServiceActionDbModel> ServicesActions {  get; set; }
        public virtual DbSet<ConfigurationDbModel> Configurations { get; set;}
    }
}
