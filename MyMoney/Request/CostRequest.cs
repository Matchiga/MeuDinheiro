namespace MyMoney.Request;

public record CostRequest(int Id, string Description, double Value, DateTime Date, int CategoryId = 8);
