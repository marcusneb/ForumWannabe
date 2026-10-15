using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ServerEntities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CommentController(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            await postRepo.GetSingleAsync(request.PostId);
        }
        catch (Exception)
        {
            return NotFound($"Post with id {request.PostId} not found");
        }

        User author;
        try
        {
            author = await userRepo.GetSingleAsync(request.UserId);
        }
        catch (Exception)
        {
            return NotFound($"User with id {request.UserId} not found");
        }

        Comment comment = new Comment { Body = request.Body, UserId = request.UserId, PostId = request.PostId };
        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId,
            AuthorUsername = author.Username
        };

        return Created($"/Comment/{dto.Id}", dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingle([FromRoute] int id)
    {
        Comment comment;
        try
        {
            comment = await commentRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"Comment with id {id} not found");
        }

        User author = await userRepo.GetSingleAsync(comment.UserId);

        CommentDto dto = new CommentDto
        {
            Id = comment.Id,
            AuthorUsername = author.Username,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };
        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany([FromQuery] string? userName,
        [FromQuery] int? postId,
        [FromQuery] int? userId)
    {
        IEnumerable<Comment> comments = commentRepo.GetMany();
        List<User> users = userRepo.GetMany().ToList();

        if (postId is not null)
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        if (userId is not null)
        {
            comments = comments.Where(c => c.UserId == userId);
        }

        if (userName is not null)
        {
            List<int> matchingIds = users
                .Where(u => u.Username.Contains(userName))
                .Select(u => u.UserId)
                .ToList();

            comments = comments.Where(c => matchingIds.Contains(c.UserId));
        }

        List<CommentDto> dtos = comments
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Body = c.Body,
                PostId = c.PostId,
                UserId = c.UserId,
                AuthorUsername = users.First(u => u.UserId == c.UserId).Username
            })
            .ToList();
        return Ok(dtos);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        Comment comment;
        try
        {
            comment = await commentRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"Comment with id {id} not found");
        }


        comment.Body = request.Body;
        await commentRepo.UpdateAsync(comment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
        }
        catch
        {
            return NotFound($"Comment with id {id} not found");
        }

        return NoContent();
    }
}