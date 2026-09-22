using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreateCommentView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task CreateComment()
    {
        Console.WriteLine("What is your id?");
        string? userIdInput = Console.ReadLine();
        int userId = int.Parse(userIdInput);

        User? user = userRepository.GetMany().FirstOrDefault(u => u.UserId == userId);
        if (user is null)
        {
            Console.WriteLine("There is no user with id: " + userId + ", try again.");
            return;
        }

        Console.WriteLine("Enter the post id you want to comment on");
        string? postIdInput = Console.ReadLine();
        int postId = int.Parse(postIdInput);

        Post? post = postRepository.GetMany().FirstOrDefault(p => p.Id == postId);
        if (post is null)
        {
            Console.WriteLine("There is no post with id: " + postId + ", try again.");
            return;
        }

        Console.WriteLine("Type the comment");
        string? body = Console.ReadLine();
        
        Comment comment = new Comment {Body = body, UserId = userId, PostId = postId};
        
        Comment createdComment = await commentRepository.AddAsync(comment);
        
        Console.WriteLine($"Created comment with id {createdComment.Id}");
    }
}