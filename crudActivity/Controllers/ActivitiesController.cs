using Microsoft.AspNetCore.Mvc;
using crudActivity.Dtos;
using crudActivity.Services;

namespace crudActivity.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : Controller
    {
        private readonly ActivityRepository _repo;

        public ActivitiesController(ActivityRepository repo) => _repo = repo;

        // get all filter
        [HttpGet]
        public async Task<ActionResult<PagedResult<ActivityCardDto>>> List([FromQuery] ActivityQuery q)
        {
            var (items, total) = await _repo.ListAsync(q);

            var result = new PagedResult<ActivityCardDto>(
                q.Page, q.PageSize, total,
                items.Select(p => new ActivityCardDto(
                    p.Id.ToString(), p.Title, p.Slug, p.CoverUrl, p.Excerpt, p.PublishedAt, p.Categories //field ที่จะคืนออกไปให้ user
                )).ToList()
            );

            return Ok(result);
        }

        // get detail by slug
        [HttpGet("{slug}")]
        public async Task<ActionResult<ActivityDetailDto>> Detail(string slug)
        {
            var p = await _repo.GetBySlugAsync(slug);
            if (p is null) return NotFound();

            return Ok(new ActivityDetailDto(
                p.Id.ToString(), p.Title, p.Slug, p.CoverUrl, p.Excerpt, p.Content,
                p.PublishedAt, p.Categories, p.Images.OrderBy(i => i.Order).Select(i => i.Url).ToList()
            ));
        }

        // create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Title and Content are required.");

            var created = await _repo.CreateAsync(dto);
            return CreatedAtAction(nameof(Detail),
            new { slug = created.Slug },
            new
            {
                message = "Activity created successfully.",
                id = created.Id.ToString(),
                slug = created.Slug
            });
        }

        // update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ActivityUpdateDto dto)
        {
            var ok = await _repo.UpdateAsync(id, dto);
            if (!ok)
                return NotFound(new { message = $"Activity with id '{id}' not found." });

            return Ok(new { message = "Activity updated successfully." });
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _repo.SoftDeleteAsync(id);
            if (!ok)
                return NotFound(new { message = $"Activity with id '{id}' not found or already deleted." });

            return Ok(new { message = "Activity deleted successfully." });
        }
    }
}
