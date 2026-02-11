using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using RuangBooking.Data;
using RuangBooking.Models;

namespace RuangBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class BookRoomController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookRoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetBookedRooms")]
    public IActionResult Get()
    {
        var bookedRooms = _context.BookedRooms.Where(br => !br.IsDeleted).ToListAsync();
        return Ok(bookedRooms);
    }

    [HttpGet("{id}", Name = "GetBookedRoomByIdRoom")]
    public IActionResult Get(int id)
    {
        var bookedRoom = _context.BookedRooms.Where(br => br.RoomId == id).ToListAsync();
        if (bookedRoom == null)
        {
            return NotFound();
        }
        return Ok(bookedRoom);
    }

    [HttpPost(Name = "PostBookRoom")]
    public IActionResult Post(BookRoom bookRoom)
    {
        _context.BookedRooms.Add(bookRoom);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = bookRoom.Id }, bookRoom);
    }

    [HttpPut("{id}", Name = "PutBookRoom")]
    public IActionResult Put(int id, BookRoom bookRoom)
    {
        if (bookRoom.Id == 0){
            bookRoom.Id = id;
        }

        _context.Entry(bookRoom).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(bookRoom);
    }

    [HttpDelete("{id}", Name = "DeleteBookRoom")]
    public IActionResult Delete(int id)
    {
        var bookRoom = _context.BookedRooms.Find(id);
        if (bookRoom == null)
        {
            return NotFound();
        }

        // Soft delete
        bookRoom.IsDeleted = true;
        _context.SaveChanges();
        return Ok(new { message = "Booking Deleted successfully." });
    }
}