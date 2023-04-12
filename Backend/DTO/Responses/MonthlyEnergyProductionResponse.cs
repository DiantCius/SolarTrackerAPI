namespace Backend.DTO.Responses
{
    public record MonthlyProduction(int month, int year, float production);
    public record MonthlyEnergyProductionsResponse(string serialNumber, List<MonthlyProduction> monthlyProductions);
}
