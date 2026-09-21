using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class CreatePostView
{
  private readonly IPostRepository postRepository;

  public CreatePostView(IPostRepository postRepository)
  {
    this.postRepository = postRepository;
  }

  public async Task CreatePost()
  {

    Console.WriteLine("What is your user id?");
    string? userIdInput = Console.ReadLine();
    int userId = int.Parse(userIdInput);
    
    Console.WriteLine("What's the title of your post?");
    string? title = Console.ReadLine();
    
    Console.WriteLine("What would you like to post?");
    string? content = Console.ReadLine();
    
    Post post = new Post {Title = title, Content = content, UserId = userId};
    Post createdPost = await postRepository.AddAsync(post);
    Console.WriteLine("The post was successfully created!");
  }
    
}