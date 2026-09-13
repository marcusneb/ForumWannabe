namespace InMemoryRepositories;

using ServerEntities;
using RepositoryContracts;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);

    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.FirstOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                            $"Post with id {post.Id} was not found.");
                
        }
        posts.Remove(existingPost);
        posts.Add(post);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.FirstOrDefault(p => p.Id == id);
        if (postToRemove is null)
            {
            throw new InvalidOperationException($"Post with id {id} was not found.");
            }
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.FirstOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with id {id} was not found.");
        }
        
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}