using DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<CommentDto> GetSingleAsync(int id);
    Task<List<CommentDto>> GetManyAsync(int? postId = null, int? userId = null, string? userName = null);
    Task UpdateAsync(int id, UpdateCommentDto request);
    Task DeleteAsync(int id);
}