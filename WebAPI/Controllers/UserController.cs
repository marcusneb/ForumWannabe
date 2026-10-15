using DTOs;
using ServerEntities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;


namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UserController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        bool taken = userRepo.GetMany().Any(u => u.Username == request.UserName);
        if (taken)
        {
            return BadRequest($"Username '{request.UserName}' is already taken");
        }

        User user = new User { Username = request.UserName, Password = request.Password };
        User created = await userRepo.AddAsync(user);

        UserDto dto = new UserDto
        {
            Id = created.UserId,
            Username = created.Username
        };
        return Created($"/Users/{dto.Id}", dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetSingle([FromRoute] int id)
    {
        User user;
        try
        {
            user = await userRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"User with id {id} not found");
        }

        UserDto dto = new UserDto
        {
            Id = user.UserId,
            Username = user.Username
        };

        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string? userName)
    {
        IEnumerable<User> users = userRepo.GetMany();
        if (userName is not null)
        {
            users = users.Where(u => u.Username.Contains(userName));
        }

        List<UserDto> dtos = users
            .Select(u => new UserDto { Id = u.UserId, Username = u.Username })
            .ToList();

        return Ok(dtos);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        User user;
        try
        {
            user = await userRepo.GetSingleAsync(id);
        }
        catch
        {
            return NotFound($"User with id {id} not found");
        }

        bool taken = userRepo.GetMany()
            .Any(u => u.Username == request.Username && u.UserId != id);

        if (taken)
        {
            return BadRequest($"Username '{request.Username}' is already taken");
        }

        user.Username = request.Username;
        user.Password = request.Password;
        await userRepo.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        
        try
        {
         await userRepo.DeleteAsync(id);
            
        }
        catch
        {
            return NotFound($"User with id {id} not found");
        }

        return NoContent();
    }
    
}