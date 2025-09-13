using crudActivity.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace crudActivity.Services
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string ActivitiesCollection { get; set; } = "activities";
    }
    public class MongoContext
    {
        public IMongoDatabase Db { get; }
        public IMongoCollection<Activity> Activities { get; }

        public MongoContext(IOptions<MongoSettings> opt)
        {
            var client = new MongoClient(opt.Value.ConnectionString);
            Db = client.GetDatabase(opt.Value.Database);
            Activities = Db.GetCollection<Activity>(opt.Value.ActivitiesCollection);

            EnsureIndexes();
        }

        private void EnsureIndexes()
        {
            // unique slug
            Activities.Indexes.CreateOne(new CreateIndexModel<Activity>(
                Builders<Activity>.IndexKeys.Ascending(x => x.Slug),
                new CreateIndexOptions { Unique = true }
            ));

            // สำหรับ list query
            Activities.Indexes.CreateOne(new CreateIndexModel<Activity>(
                Builders<Activity>.IndexKeys
                    .Ascending(x => x.Status)
                    .Descending(x => x.PublishedAt)
            ));

            // Text ค้นหา (title, excerpt, content)
            Activities.Indexes.CreateOne(new CreateIndexModel<Activity>(
                Builders<Activity>.IndexKeys.Text(x => x.Title)
                    .Text(x => x.Excerpt)
                    .Text(x => x.Content)
            ));
        }
    }
}
