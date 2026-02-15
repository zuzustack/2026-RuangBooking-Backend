using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using RuangBooking.Data;
using RuangBooking.Models;
using RuangBooking.Requests;

namespace RuangBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetUsers")]
    public IActionResult Get()
    {
        var users = _context.Users.Where(u => !u.IsDeleted).ToList();
        return Ok(users);
    }

    [HttpGet("{id}", Name = "GetUserById")]
    public IActionResult Get(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("register", Name = "RegisterUser")]
    public IActionResult Register(UserRegisterRequest user)
    {
        // check if username already exists
        if (_context.Users.Any(u => u.Username == user.Username))
        {
            return BadRequest(new { message = "Username already exists" });
        }


        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        if (string.IsNullOrEmpty(user.Role))
        {
            user.Role = "User";
        }

        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password,
            Role = user.Role
        };
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return Ok(newUser);
    }

    [HttpPost("login", Name = "LoginUser")]
    public IActionResult Login(UserLoginRequest user)
    {
        var existingUser = _context.Users.SingleOrDefault(u => u.Username == user.Username);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
        {
            return Unauthorized();
        }
        return Ok(existingUser);
    }

    [HttpPut("{id}", Name = "PutUser")]
    public IActionResult Put(int id, User user)
    {
        if (user.Id == 0){
            user.Id = id;
        }

        _context.Entry(user).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(user);
    }

    [HttpDelete("{id}", Name = "DeleteUser")]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.IsDeleted = true;
        _context.SaveChanges();
        return NoContent();
    }
}
