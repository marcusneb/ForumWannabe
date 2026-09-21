namespace InMemoryRepositories;

using ServerEntities;
using RepositoryContracts;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;


    public PostInMemoryRepository()
    {
        posts = new List<Post>();
        
        posts.Add(new Post {Id = 1, Title = "Why is Java overrated?", Content = "Just so",  UserId = 1});
        posts.Add(new Post {Id = 2, Title = "Is AI gonna take my job?", Content = "Maybe yes, we're screwed",  UserId = 2});
        posts.Add(new Post {Id = 3, Title = "Best career after failing SWE?", Content = "I am thinking about farmer, what do y'all think?",  UserId = 3});
    }
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