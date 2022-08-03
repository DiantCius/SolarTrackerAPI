using Backend.Models;

namespace Backend.DTO.Requests
{
    public record UpdatePowerPlantStatusRequest(string SerialNumber, ConnectionStatus ConnectionStatus);
}
