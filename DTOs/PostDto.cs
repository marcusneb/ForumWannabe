namespace DTOs;

public class PostDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string AuthorUsername { get; set; }
}