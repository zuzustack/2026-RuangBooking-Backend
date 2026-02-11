namespace RuangBooking.Models;

public class BookRoom
{
    public int Id { get; set; } // Primary key for the database

    public int RoomId { get; set; }

    public int BookedBy { get; set; }

    public int ApprovedBy { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    // softdelete and dont show at results
    public bool IsDeleted { get; set; } = false;
}