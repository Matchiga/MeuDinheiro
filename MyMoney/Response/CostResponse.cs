namespace MyMoney.Response;

public record CostResponse(int Id, string Description, double Value, DateTime Date, int CategoryId, string CategoryName);