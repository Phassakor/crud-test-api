using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace crudActivity.Models
{
    public class Activity
    {
        [BsonId] public ObjectId Id { get; set; }

        [BsonRequired] public string Title { get; set; } = default!;
        [BsonRequired] public string Slug { get; set; } = default!;

        public string? Excerpt { get; set; }
        public string Content { get; set; } = default!;

        public string? CoverUrl { get; set; }

        public List<ActivityImage> Images { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        // draft | published | archived
        public string Status { get; set; } = "draft";
        public DateTime? PublishedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ActivityImage
    {
        public string Url { get; set; } = "";
        public int Order { get; set; } = 0;
    }
}
