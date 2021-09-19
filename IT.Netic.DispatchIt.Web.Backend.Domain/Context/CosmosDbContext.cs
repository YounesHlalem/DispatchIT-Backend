using IT.Netic.DispatchIt.Web.Backend.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Authentication;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Context
{
    /// <summary>
    /// A seperate class to configure and create a client connection with the CosmosDb database.
    /// Returned as a MongoDatabase for easy use in the repository.
    /// </summary>
    public class CosmosDbContext
    {
        private readonly AppOptions _appOptions;

        private string dbName = "DispatchItDeliveries";

        public CosmosDbContext(IOptions<AppOptions> appOptions)
        {
            _appOptions = appOptions.Value;
        }

        public IMongoDatabase CreateCosmosDbContext()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(_appOptions.CosmosHost, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            settings.RetryWrites = false;

            MongoIdentity identity = new MongoInternalIdentity(dbName, _appOptions.CosmosUserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(_appOptions.CosmosPassword);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            return database;
        }
    }
}
