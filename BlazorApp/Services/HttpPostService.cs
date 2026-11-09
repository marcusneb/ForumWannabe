using DTOs;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("Post", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        PostDto? post = await response.Content.ReadFromJsonAsync<PostDto>();
        return post!;
    }

    public async Task<PostDto> GetSingleAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"Post/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        PostDto? post = await response.Content.ReadFromJsonAsync<PostDto>();
        return post!;
    }

    public async Task<List<PostDto>> GetManyAsync(string? title = null, int? userId = null, string? userName = null)
    {
        List<string> filters = new List<string>();

        if (!string.IsNullOrEmpty(title))
            filters.Add($"title={Uri.EscapeDataString(title)}");
        if (userId is not null)
            filters.Add($"userId={userId}");
        if (!string.IsNullOrEmpty(userName))
            filters.Add($"userName={Uri.EscapeDataString(userName)}");

        string url = "Post";
        if (filters.Count > 0)
            url += "?" + string.Join("&", filters);
        HttpResponseMessage response = await client.GetAsync(url);
        List<PostDto>? posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();
        return posts ?? new List<PostDto>();
    }

    public async Task UpdateAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"Post/{id}", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Post/{id}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}