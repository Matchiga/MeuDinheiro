namespace MyMoney.Request;

public record RevenueRequestEdit(int Id, string Description, double Value, DateTime Date) : RevenueRequest(Id, Description, Value, Date);