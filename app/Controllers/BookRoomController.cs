using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using RuangBooking.Data;
using RuangBooking.Models;
using RuangBooking.Requests;

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

        // Join BookedRooms with Rooms, user to get the room name, and Bookedby, for approved get the username if approved by is not null
        var bookedRooms = _context.BookedRooms
            .Where(br => !br.IsDeleted)
            .Join(_context.Rooms, br => br.RoomId, r => r.Id, (br, r) => new { br, r })
            .Join(_context.Users, br => br.br.BookedBy, u => u.Id, (br, u) => new { br.br, br.r, BookedByUser = u })
            .GroupJoin(_context.Users, br => br.br.ApprovedBy, u => u.Id, (br, u) => new { br.br, br.r, br.BookedByUser, ApprovedByUser = u.FirstOrDefault() })
            .Select(br => new {
                br.br.Id,
                RoomName = br.r.Name,
                BookedBy = br.BookedByUser.Username,
                ApprovedBy = br.br.ApprovedBy != null ? br.ApprovedByUser.Username : null,
                br.br.StartTime,
                br.br.EndTime
            })
            .ToList();
        return Ok(new { data = bookedRooms });
    }

    [HttpGet("{id}", Name = "GetBookedRoomByIdRoom")]
    public IActionResult Get(int id)
    {
        var bookedRoom = _context.BookedRooms.Where(br => br.RoomId == id).ToList();
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

    // Approve booking
    [HttpPost("{id}/approve", Name = "ApproveBookRoom")]
    public IActionResult Approve(ApproveRequest request, int id)
    {
        var bookRoom = _context.BookedRooms.Find(id);
        if (bookRoom == null) {
            return NotFound();
        }

        bookRoom.ApprovedBy = request.approvedBy;
        _context.SaveChanges();
        return Ok(new { message = "Booking approved successfully." });
    }
}