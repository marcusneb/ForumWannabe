using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ServerEntities;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{

    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {

        User author;
        try
        {
            author = await userRepo.GetSingleAsync(request.UserId);
        }

        catch (Exception)
        {
            return NotFound($"User with id {request.UserId} not found");
        }
        Post post = new Post {Title = request.Title, Content = request.Content, UserId = request.UserId};
        Post created = await postRepo.AddAsync(post);

        PostDto dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Content = created.Content,
            AuthorUsername = author.Username
            
        };
        return Created($"/Post/{dto.Id}", dto);
    }

    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetSingle([FromRoute] int id)
    {
        Post post;
        try
        {
            post = await postRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"Post with id {id} not found");
        }

        User author = await userRepo.GetSingleAsync(post.UserId);

        PostDto dto = new PostDto
        {
            Id = post.Id,
            AuthorUsername = author.Username,
            Content = post.Content,
            Title = post.Title
        };
        return Ok(dto);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetMany([FromQuery] string? userName,
                                                      [FromQuery] string? title, 
                                                      [FromQuery] int? userId)
    {
        IEnumerable<Post> posts = postRepo.GetMany();
        List<User> users = userRepo.GetMany().ToList();

        if (title is not null)
        {
            
            posts = posts.Where(p => p.Title.Contains(title));
        }
        
        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        if (userName is not null)
        {
            List<int> matchingIds = users
                .Where(u => u.Username.Contains(userName))
                .Select(u => u.UserId)
                .ToList();

            posts = posts.Where(p => matchingIds.Contains(p.UserId));
        }

        List<PostDto> dtos = posts
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                AuthorUsername = users.First(u => u.UserId == p.UserId).Username
            })
            .ToList();
        return Ok(dtos);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        Post post;
        try
        {
            post = await postRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"Post with id {id} not found");
        }


        post.Title = request.Title;
        post.Content = request.Content;
        await postRepo.UpdateAsync(post);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
        }
        catch
        {
            return NotFound($"Post with id {id} not found");
        }

        return NoContent();
    }

    
    
}