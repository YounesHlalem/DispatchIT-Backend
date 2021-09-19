using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using IT.Netic.DispatchIt.Web.Backend.Functions;
using System.IO;
using IT.Netic.DispatchIt.Web.Backend.Common.Options;
using IT.Netic.DispatchIt.Web.Backend.Domain.Context;
using IT.Netic.DispatchIt.Web.Backend.Domain.Repository;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using IT.Netic.DispatchIt.Web.Backend.Services;
using IT.Netic.DispatchIt.Web.Backend.Services.MappingProfiles;

[assembly: FunctionsStartup(typeof(Startup))]
namespace IT.Netic.DispatchIt.Web.Backend.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();

            builder.Services.Configure<AppOptions>(options => options.CosmosUserName = configuration.GetConnectionString("CosmosDbUserName"));
            builder.Services.Configure<AppOptions>(options => options.CosmosHost = configuration.GetConnectionString("CosmosDbHost"));
            builder.Services.Configure<AppOptions>(options => options.CosmosPassword = configuration.GetConnectionString("CosmosDbPassword"));
            builder.Services.Configure<AppOptions>(options => options.SqlServerName = configuration.GetConnectionString("SqlServerName"));
            builder.Services.Configure<AppOptions>(options => options.SqlDatabaseName = configuration.GetConnectionString("SqlDatabaseName"));
            builder.Services.Configure<AppOptions>(options => options.SqlUserName = configuration.GetConnectionString("SqlUserName"));
            builder.Services.Configure<AppOptions>(options => options.SqlPassword = configuration.GetConnectionString("SqlPassword"));

            builder.Services.AddSingleton<IDbContextCreator, DbContextCreator>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddSingleton<CosmosDbContext, CosmosDbContext>();
            builder.Services.AddScoped<DeliveryrequestRepository, DeliveryrequestRepository>();
            builder.Services.AddScoped<IDeliveryrequestService, DeliveryrequestService>();
            builder.Services.AddScoped<IValidationService, ValidationService>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MainMappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }
    }
}
