using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class CreatePostView
{
  private readonly IPostRepository postRepository;
  private readonly IUserRepository userRepository;

  public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
  {
    this.postRepository = postRepository;
    this.userRepository = userRepository;
  }

  public async Task CreatePost()
  {
    

    Console.WriteLine("What is your user id?");
    string? userIdInput = Console.ReadLine();
    int userId = int.Parse(userIdInput);

    User? user = userRepository.GetMany().FirstOrDefault(u => u.UserId == userId);
    if (user is null)
    {
      Console.WriteLine("There is no user with id: " + userId + ", try again.");
      return;
    }
    
    Console.WriteLine("What's the title of your post?");
    string? title = Console.ReadLine();
    
    Console.WriteLine("What would you like to post?");
    string? content = Console.ReadLine();
    
    Post post = new Post {Title = title, Content = content, UserId = userId};
    Post createdPost = await postRepository.AddAsync(post);
    Console.WriteLine($"The post with id {post.Id}  was successfully created!");
  }
    
}