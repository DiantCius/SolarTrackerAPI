namespace Backend.DTO
{
    public record IndicationDto(string SerialNumber, float Azimuth,float Elevation, float WindSpeed, int[] State);

}
