namespace Backend.DTO.Requests
{
    public record UpdateCodeRequest(int Id, string SerialNumber, bool IsUsed);
}
