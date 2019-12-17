using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Penthouse_Security
{
    class DatabaseConnector
    {
        private MongoClient client;
        internal IMongoDatabase metricsDatabase { get; }
        public DatabaseConnector(string mongoPasswd)
        {
            Log.Info("Connecting to mongo database...");
            client = new MongoClient("mongodb+srv://securityBot:" + mongoPasswd + "@penthousesecurity-uglzv.azure.mongodb.net/test?retryWrites=true&w=majority");

            Log.Info("Client cluster: " + client.Cluster.ToString());

            metricsDatabase = client.GetDatabase("Metrics");
            Log.Info("Metrics database object: " + metricsDatabase.ToString());
        }

        internal IMongoCollection<BsonDocument> GetSpinCollection()
        {
            return metricsDatabase.GetCollection<BsonDocument>("Spin");
        }
    }
}
