namespace Backend.DTO.Requests
{
    public record RegisterRequest (string Username, string Email, string Password, string ConfirmPassword);
}
