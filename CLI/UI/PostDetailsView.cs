using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class PostDetailsView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public PostDetailsView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        
    }


    public async Task ViewPost()
    {
        Console.WriteLine("Enter the post id you want to see");
        string? postIdInput = Console.ReadLine();
        int postId = int.Parse(postIdInput);

       Post post = await postRepository.GetSingleAsync(postId);

       Console.WriteLine($"\nTitle:{post.Title}");
       Console.WriteLine($"Content:{post.Content}");
       
       IEnumerable<Comment> comments = commentRepository.GetMany().Where(c => postId == c.PostId);

       foreach (Comment comment in comments)
       {
           Console.WriteLine(comment.Body);
       }
    }
    
}