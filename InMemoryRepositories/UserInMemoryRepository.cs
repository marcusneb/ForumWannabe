using RepositoryContracts;
using ServerEntities;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users;
    
    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Count + 1;
        users.Add(user);
        return Task.FromResult(user);
        
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.FirstOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with the id {user.UserId} was not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.FirstOrDefault(u => u.UserId == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with id {id} was not found.");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.FirstOrDefault(u => u.UserId == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with id {id} was not found.");
        }

        return Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}