using DTOs;

namespace BlazorApp.Services;


public class HttpUserService : IUserService
{
    private HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        //sent the request, with the DTO as JSON
        HttpResponseMessage response = await client.PostAsJsonAsync("User", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user!;


    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"User/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>();
        return user!;
    }

    public async Task<List<UserDto>> GetManyAsync(string? userName = null)
    {
        string url = "User";
        if (!string.IsNullOrEmpty(userName))
        {
            url += $"?userName={Uri.EscapeDataString(userName)}";
        }
        
        HttpResponseMessage response = await client.GetAsync(url);
        List<UserDto>? users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        return users ?? new List<UserDto>();
    }

    public async Task UpdateAsync(int id, UpdateUserDto request)
    {
        
        HttpResponseMessage response = await client.PutAsJsonAsync($"User/{id}", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        
    }

    public async Task DeleteAsync(int id)
    {
        
        HttpResponseMessage response = await client.DeleteAsync($"User/{id}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}