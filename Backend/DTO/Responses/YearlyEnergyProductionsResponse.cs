namespace Backend.DTO.Responses
{
    public record YearlyProduction(int year, float production);
    public record YearlyEnergyProductionsResponse(string serialNumber, List<YearlyProduction> monthlyProductions);
}
