namespace RuangBooking.Models;

public class User
{
    public int Id { get; set; } // Primary key for the database

    public string Username { get; set; }

    public string Password { get; set; }

    // e.g., "Admin", "User"
    public string Role { get; set; }

    // softdelete and dont show at results
    public bool IsDeleted { get; set; } = false;
}