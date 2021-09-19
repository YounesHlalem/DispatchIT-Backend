using IT.Netic.DispatchIt.Web.Backend.Domain.Context;
using IT.Netic.DispatchIt.Web.Backend.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public class DeliveryrequestRepository
    {
        protected readonly CosmosDbContext DbContext;
        protected readonly IMongoDatabase DispatchItDatabase;

        public DeliveryrequestRepository(CosmosDbContext dbContext)
        {
            DbContext = dbContext;
            DispatchItDatabase = DbContext.CreateCosmosDbContext();
        }

        private IMongoCollection<Deliveryrequest> GetDeliveryrequestCollection()
        {
            var DeliveryrequestCollection = DispatchItDatabase.GetCollection<Deliveryrequest>("Deliveryrequests");
            return DeliveryrequestCollection;
        }

        private IMongoCollection<Message> GetMessageCollection()
        {
            var MessageCollection = DispatchItDatabase.GetCollection<Message>("Messages");
            return MessageCollection;
        }

        public List<Message> GetAllMessagesByUser(string user)
        {
            try
            {
                var collection = GetMessageCollection();
                var filter = Builders<Message>.Filter.Eq("User", user);
                var result = collection.Find(filter).ToList();

                return result;
            }
            catch (MongoConnectionException)
            {
                return new List<Message>();
            }
        }

        public bool CreateMessage(Message mess)
        {
            var collection = GetMessageCollection();
            try
            {
                collection.InsertOne(mess);
                return true;
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

        public bool CreateDeliveryrequest(Deliveryrequest delReq)
        {
            var collection = GetDeliveryrequestCollection();
            try
            {
                delReq.deliveryId = (int)collection.CountDocumentsAsync(new BsonDocument()).Result + 1;
                collection.InsertOne(delReq);
                return true;
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

        public List<Deliveryrequest> GetAllDeliveryrequests()
        {
            try
            {
                var collection = GetDeliveryrequestCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<Deliveryrequest>();
            }
        }

        public List<Deliveryrequest> GetAllDeliveryrequestsForCompany(string company)
        {
            try
            {
                var collection = GetDeliveryrequestCollection();
                var filter = Builders<Deliveryrequest>.Filter.Eq("companyVat", company);
                var result = collection.Find(filter).ToList(); //.SingleAsync();

                return result;
            }
            catch (MongoConnectionException)
            {
                return new List<Deliveryrequest>();
            }
        }

        public List<Deliveryrequest> GetDeliveryrequestForId(int id)
        {
            try
            {
                var collection = GetDeliveryrequestCollection();
                var filter = Builders<Deliveryrequest>.Filter.Eq("companyId", id);
                var result = collection.Find(filter).ToList(); //.SingleAsync();

                return result;
            }
            catch (MongoConnectionException)
            {
                return new List<Deliveryrequest>();
            }
        }

        public Deliveryrequest GetDelivery(int id)
        {
            try
            {
                var collection = GetDeliveryrequestCollection();
                var filter = Builders<Deliveryrequest>.Filter.Eq("deliveryId", id);
                var result = collection.Find(filter).FirstOrDefault();

                return result;
            }
            catch (MongoConnectionException)
            {
                return null;
            }
        }

        public bool UpdateApprovedDelivery(int id)
        {
            var collection = GetDeliveryrequestCollection();
            var filter = Builders<Deliveryrequest>.Filter.Eq("deliveryId", id);
            var update = Builders<Deliveryrequest>.Update
                .Set("isVerifiedBySender", true)
                .Set("status", "Approved");
            
            collection.UpdateOne(filter, update);
            return true;
        }

        public bool UpdateRejectedDelivery(int id)
        {
            var collection = GetDeliveryrequestCollection();
            var filter = Builders<Deliveryrequest>.Filter.Eq("deliveryId", id);
            var update = Builders<Deliveryrequest>.Update
                .Set("claimedBy", 0)
                .Set("status", "Open");

            collection.UpdateOne(filter, update);
            return true;
        }

        public bool UpdateDelivery(Deliveryrequest delReq)
        {
            DeleteDelivery(delReq.deliveryId);
            CreateDeliveryrequest(delReq);
            return true;
        }

        public bool DeleteDelivery(int id)
        {
            var collection = GetDeliveryrequestCollection();
            var filter = Builders<Deliveryrequest>.Filter.Eq("deliveryId", id);

            collection.DeleteOne(filter);
            return true;
        }
    }
}
