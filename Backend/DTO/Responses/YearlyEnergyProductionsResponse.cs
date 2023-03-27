namespace Backend.DTO.Responses
{
    public record YearlyProduction(int year, int production);
    public record YearlyEnergyProductionsResponse(string serialNumber, List<YearlyProduction> monthlyProductions);
}
