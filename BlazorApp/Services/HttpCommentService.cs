using DTOs;

namespace BlazorApp.Services;


public class HttpCommentService : ICommentService
{
    private HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }


    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("Comment", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        CommentDto? comment = await response.Content.ReadFromJsonAsync<CommentDto>();
        return comment!;
    }

    public async Task<CommentDto> GetSingleAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"Comment/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        CommentDto? comment = await response.Content.ReadFromJsonAsync<CommentDto>();
        return comment!;
    }

    public async Task<List<CommentDto>> GetManyAsync(int? postId = null, int? userId = null, string? userName = null)
    {
        List<string> filters = new List<string>();
        
        if (postId is not null)
            filters.Add($"postId={postId}");
        if (userId is not null)
            filters.Add($"userId={userId}");
        if (!string.IsNullOrEmpty(userName))
            filters.Add($"userName={Uri.EscapeDataString(userName)}");
        string url = "Comment";
        if (filters.Count > 0)
            url += "?" + string.Join("&", filters);
        HttpResponseMessage response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        List<CommentDto>? comments = await response.Content.ReadFromJsonAsync<List<CommentDto>>();
        return comments ?? new List<CommentDto>();
    }

    public async Task UpdateAsync(int id, UpdateCommentDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"Comment/{id}", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Comment/{id}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}