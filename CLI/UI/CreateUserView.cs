using RepositoryContracts;
using ServerEntities;

namespace CLI.UI;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUser()
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        User user = new User { Username = username, Password = password };
        
       User createdUser =  await userRepository.AddAsync(user);
       Console.WriteLine($"User with id  {createdUser.UserId} created.");
    }
}