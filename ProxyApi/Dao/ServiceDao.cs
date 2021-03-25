using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using ProxyApi.Models;
using WsFirst.Models.Database;
using ProxyApi.Dao.Interfaces;

namespace ProxyApi.Dao
{
    public class ServiceDao : IWsService
    {
        private readonly IMongoCollection<Service> _service;

        public ServiceDao(IProxyDatabaseSettings settings)
        {
            //Récupération des informations dans le appsettings.json

            var mongoCredential = MongoCredential.CreateCredential(settings.DatabaseName, settings.UserName, settings.Password);

            MongoClientSettings mongoSettings = new MongoClientSettings()
            {
                Credential = mongoCredential,
                Server = new MongoServerAddress(settings.ConnectionString)
            };

            MongoClient client = new MongoClient(mongoSettings);
            var database = client.GetDatabase(settings.DatabaseName);

            _service = database.GetCollection<Service>(settings.ServiceCollectionName);
        }


        public Service GetService(string requestUrl)
        {
            //return  _service.Find<Service>(service => requestUrl.Contains(service.RequestValue)).FirstOrDefault();

            var pipe = new[] { new BsonDocument("$addFields",
                        new BsonDocument("matchingIndex",
                        new BsonDocument("$indexOfCP",
                        new BsonArray
                                    {
                                        requestUrl,
                                        "$RequestValue"
                                    }))),
                        new BsonDocument("$match",
                        new BsonDocument("matchingIndex",
                        new BsonDocument("$gt", -1))),
                        //new BsonDocument("$limit", 1),
                        new BsonDocument("$unset", "matchingIndex")};


            return _service.Aggregate<Service>(pipe).First();
        }
    }
}
