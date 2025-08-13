namespace MyMoney.Response;

public class SummaryResponse
{
    public double TotalRevenues { get; set; }
    public double TotalCost { get; set; }
    public double FinalBalance { get; set; }
    public Dictionary<string, double> ExpensesByCategory { get; set; } = new();
}
