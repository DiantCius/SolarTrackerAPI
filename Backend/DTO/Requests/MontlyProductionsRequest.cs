using Backend.Models;

namespace Backend.DTO.Requests
{
    public record MonthlyProductionsRequest(string serialNumber, int month, int year);
}