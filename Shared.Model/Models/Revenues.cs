namespace Shared.Models;

public class Revenues
{
    public int Id { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public DateTime Date { get; set; }
    public Revenues(int id, string description, double value, DateTime date)
    {
        Id = id;
        Description = description;
        Value = value;
        Date = date;
    }
}
