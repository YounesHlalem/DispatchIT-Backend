using System;
using IT.Netic.DispatchIt.Web.Backend.Common.Options;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Context
{
    /// <summary>
    /// The database context for the mysql database, named DispathIt, to configure and create the connection and database
    /// </summary>
    public class DispatchItDbContext : DbContext, IDispatchItDbContext
    {
        private readonly AppOptions _appOptions; 

        public DispatchItDbContext(DbContextOptions<DispatchItDbContext> ctxOptions, IOptions<AppOptions> appOptions) : base(ctxOptions)
        {
            _appOptions = appOptions.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var builder = new MySqlConnectionStringBuilder
            {
                Server = _appOptions.SqlServerName,
                Database = _appOptions.SqlDatabaseName,
                UserID = _appOptions.SqlUserName,
                Password = _appOptions.SqlPassword,
                SslMode = MySqlSslMode.Required,
            };
            var connectionString = builder.ConnectionString;
            optionsBuilder.UseMySQL(connectionString, opts =>
            {
                opts.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
                //opts.EnableRetryOnFailure();
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).ValueGeneratedOnAdd();
                entity.HasKey(e => e.CompanyId);
                entity.Property(p => p.VatNr).IsRequired();
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.PhoneNr);
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.Owner).IsRequired();
            });
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).ValueGeneratedOnAdd();
                entity.HasKey(e => e.AddressId);
                entity.Property(p => p.Country).IsRequired();
                entity.Property(p => p.Zipcode).IsRequired();
                entity.Property(p => p.City);
                entity.Property(p => p.Streetname).IsRequired();
                entity.Property(p => p.Number).IsRequired();
                entity.Property(p => p.Addition);
                entity.Property(e => e.CompanyId).IsRequired();
            });
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
