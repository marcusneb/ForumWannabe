using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class PostList
{
    private readonly IPostRepository postRepository;

    public PostList(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task GetPosts()
    {
        IQueryable<Post> posts = postRepository.GetMany();

        foreach (Post post in posts)
        {
            Console.WriteLine($"Post id: {post.Id}");
            Console.WriteLine($"Post title: {post.Title}");
        }
    }
}