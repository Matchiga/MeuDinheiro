namespace MyMoney.Request;

public record CostRequestEdit(int Id, string Description, double Value, DateTime Date, int CategoryId) : CostRequest(Id, Description, Value, Date, CategoryId);