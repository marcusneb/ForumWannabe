using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    private readonly CreateUserView createUserView;
    private readonly CreateCommentView createCommentView;
    private readonly CreatePostView createPostView;
    
    private readonly PostDetailsView postDetailsView;
    private readonly PostList postList;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;

        createUserView = new CreateUserView(userRepository);
        createCommentView = new CreateCommentView(commentRepository);
        createPostView = new CreatePostView(postRepository);
        
        postDetailsView = new PostDetailsView(postRepository, commentRepository);
        postList = new PostList(postRepository);
    }


    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nWelcome to LatestAI");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage posts");
            Console.WriteLine("3. Manage comments");
            Console.WriteLine("4. See post");
            Console.WriteLine("5. View post overview");
            Console.WriteLine("0. Exit");
            Console.WriteLine("Choose an option");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    await createUserView.CreateUser();
                    break;
                case "2":
                    await createPostView.CreatePost();
                    break;
                case "3":
                    await createCommentView.CreateComment();
                    break;
                case "4": await postDetailsView.ViewPost();
                    break;
                case "5": await postList.GetPosts();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid input, try again");
                    break;
            }
        }
    }
}