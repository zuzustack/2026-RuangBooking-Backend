using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using RuangBooking.Data;
using RuangBooking.Models;

namespace RuangBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetRooms")]
    public IActionResult Get()
    {
        var rooms = _context.Rooms.Where(r => !r.IsDeleted).ToList();
        return Ok(new { data = rooms });
    }

    [HttpGet("{id}", Name = "GetRoomById")]
    public IActionResult Get(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room == null)
        {
            return NotFound();
        }
        return Ok(room);
    }

    [HttpPost(Name = "PostRoom")]
    public IActionResult Post(Room room)
    {
        _context.Rooms.Add(room);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }

    [HttpPut("{id}", Name = "PutRoom")]
    public IActionResult Put(int id, Room room)
    {
        if (room.Id == 0){
            room.Id = id;
        }

        _context.Entry(room).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(room);
    }

    [HttpDelete("{id}", Name = "DeleteRoom")]
    public IActionResult Delete(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room == null)
        {
            return NotFound();
        }
        room.IsDeleted = true;
        _context.SaveChanges();
        return NoContent();
    }
}