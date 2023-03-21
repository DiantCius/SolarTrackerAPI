using Backend.Models;

namespace Backend.DTO.Requests
{
    public record YearlyEnergyProductionRequest(string serialNumber, int year);
}