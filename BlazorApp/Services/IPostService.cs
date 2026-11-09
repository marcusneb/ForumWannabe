using DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetSingleAsync(int id);
    Task<List<PostDto>> GetManyAsync(string? title = null, int? userId = null, string? userName = null);
    Task UpdateAsync(int id, UpdatePostDto request);
    Task DeleteAsync(int id);
}