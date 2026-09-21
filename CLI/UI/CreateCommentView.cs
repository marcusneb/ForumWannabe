using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task CreateComment()
    {
        Console.WriteLine("What is your id?");
        string? userIdInput = Console.ReadLine();
        int userId = int.Parse(userIdInput);

        Console.WriteLine("Enter the post id you want to comment on");
        string? postIdInput = Console.ReadLine();
        int postId = int.Parse(postIdInput);

        Console.WriteLine("Type the comment");
        string? body = Console.ReadLine();
        
        Comment comment = new Comment {Body = body, UserId = userId, PostId = postId};
        
        Comment createdComment = await commentRepository.AddAsync(comment);
        
        Console.WriteLine($"Created comment with id {comment.Id}");
    }
}