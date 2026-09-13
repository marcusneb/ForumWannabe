using RepositoryContracts;
using ServerEntities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments;

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Count == 0 ? 1 : comments.Max(c => c.Id) + 1;
        comments.Add(comment);
        return Task.FromResult(comment);

    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with id  {comment.Id} does not exist");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.FirstOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with id {id} was not found.");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;



    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.FirstOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with id {id} was not found.");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}