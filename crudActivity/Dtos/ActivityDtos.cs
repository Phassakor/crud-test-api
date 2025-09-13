namespace crudActivity.Dtos
{
    public record ActivityCreateDto(
      string Title,
      string? Excerpt,
      string Content,
      string? CoverUrl,
      List<string>? Categories,
      List<ActivityImageDto>? Images,
      string? Status,
      DateTime? PublishedAt
  );

    public record ActivityUpdateDto(
        string Title,
        string? Excerpt,
        string Content,
        string? CoverUrl,
        List<string>? Categories,
        List<ActivityImageDto>? Images,
        string Status,
        DateTime? PublishedAt
    );

    public record ActivityImageDto(string Url, int Order);

    public record ActivityCardDto(
        string Id,
        string Title,
        string Slug,
        string? CoverUrl,
        string? Excerpt,
        DateTime? PublishedAt,
        List<string> Categories
    );

    public record ActivityDetailDto(
        string Id,
        string Title,
        string Slug,
        string? CoverUrl,
        string? Excerpt,
        string Content,
        DateTime? PublishedAt,
        List<string> Categories,
        List<string> GalleryUrls
    );
}
