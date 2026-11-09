using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetSingleAsync(int id);
    Task<List<UserDto>> GetManyAsync(string? userName = null);
    Task UpdateAsync(int id, UpdateUserDto request);
    Task DeleteAsync(int id);
}