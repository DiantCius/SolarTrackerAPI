namespace Backend.DTO.Responses
{
    public record MonthlyProduction(int month, int year, int production);
    public record MonthlyEnergyProductionsResponse(string serialNumber, List<MonthlyProduction> monthlyProductions);
}
