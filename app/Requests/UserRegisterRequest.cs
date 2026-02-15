namespace RuangBooking.Requests;

public class UserRegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
}