namespace Backend.DTO.Requests
{
    public record DailyProductionsRequest(string serialNumber, int year, int month, int day);
}