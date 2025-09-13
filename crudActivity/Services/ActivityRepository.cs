using crudActivity.Dtos;
using crudActivity.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace crudActivity.Services
{
    public class ActivityRepository
    {
        private readonly IMongoCollection<Activity> _col;

        public ActivityRepository(MongoContext ctx) => _col = ctx.Activities;

        public async Task<(IReadOnlyList<Activity> items, long total)> ListAsync(ActivityQuery q)
        {
            var filter = Builders<Activity>.Filter.Eq(x => x.IsDeleted, false);

            if (!string.IsNullOrWhiteSpace(q.Status))
                filter &= Builders<Activity>.Filter.Eq(x => x.Status, q.Status);

            if (!string.IsNullOrWhiteSpace(q.Title))
            {
                 var keyword = q.Title.Trim();
                var regex = new BsonRegularExpression(keyword, "i");

                filter &= Builders<Activity>.Filter.Regex(x => x.Title, regex);
            }

            if (!string.IsNullOrWhiteSpace(q.Category))
                filter &= Builders<Activity>.Filter.AnyEq(x => x.Categories, q.Category);

            var find = _col.Find(filter);

            var total = await find.CountDocumentsAsync();

            var items = await find
                .SortByDescending(x => x.PublishedAt)
                .Skip((q.Page - 1) * q.PageSize)
                .Limit(q.PageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<Activity?> GetBySlugAsync(string slug, bool includeUnpublishedForManage = false)
        {
            var filter = Builders<Activity>.Filter.Eq(x => x.Slug, slug) &
                         Builders<Activity>.Filter.Eq(x => x.IsDeleted, false);

            if (!includeUnpublishedForManage)
                filter &= Builders<Activity>.Filter.Eq(x => x.Status, "published");
            //Filter.Eq เหมือน WHERE Status = 'published'

            return _col.Find(filter).FirstOrDefaultAsync();
        }

        public Task<Activity?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oid)) return Task.FromResult<Activity?>(null);
            return _col.Find(x => x.Id == oid && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Activity> CreateAsync(ActivityCreateDto dto)
        {
            var now = DateTime.UtcNow;
            var entity = new Activity
            {
                Title = dto.Title,
                Slug = SlugHelper.Slugify(dto.Title),
                Excerpt = dto.Excerpt,
                Content = dto.Content,
                CoverUrl = dto.CoverUrl,
                Categories = dto.Categories ?? new(),
                Images = (dto.Images ?? new()).Select(i => new ActivityImage { Url = i.Url, Order = i.Order }).ToList(),
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "draft" : dto.Status!,
                PublishedAt = dto.PublishedAt,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _col.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> UpdateAsync(string id, ActivityUpdateDto dto)
        {
            if (!ObjectId.TryParse(id, out var oid)) return false;

            var update = Builders<Activity>.Update
                .Set(x => x.Title, dto.Title)
                .Set(x => x.Excerpt, dto.Excerpt)
                .Set(x => x.Content, dto.Content)
                .Set(x => x.CoverUrl, dto.CoverUrl)
                .Set(x => x.Categories, dto.Categories ?? new())
                .Set(x => x.Images, (dto.Images ?? new()).Select(i => new ActivityImage { Url = i.Url, Order = i.Order }).ToList())
                .Set(x => x.Status, dto.Status)
                .Set(x => x.PublishedAt, dto.PublishedAt)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _col.UpdateOneAsync(x => x.Id == oid && !x.IsDeleted, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> SoftDeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oid)) return false;
            var result = await _col.UpdateOneAsync(x => x.Id == oid && !x.IsDeleted,
                Builders<Activity>.Update.Set(x => x.IsDeleted, true).Set(x => x.UpdatedAt, DateTime.UtcNow));
            return result.ModifiedCount > 0;
        }
    }
}
