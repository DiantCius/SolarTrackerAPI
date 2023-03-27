using Backend.Models;

namespace Backend.DTO.Requests
{
    public record DailyProductionsFromMonthRequest(string serialNumber, int month, int year);
}