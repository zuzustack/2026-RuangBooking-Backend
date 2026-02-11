namespace RuangBooking.Models;

public class Room
{
    public int Id { get; set; } // Primary key for the database

    public string Name { get; set; }

    public int Capacity { get; set; }

    public string Location { get; set; }

    // softdelete and dont show at results
    public bool IsDeleted { get; set; } = false;
}