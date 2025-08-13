using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

public class Cost
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    public Cost(int id, string description, double value, DateTime date)
    {
        Id = id;
        Description = description;
        Value = value;
        Date = date;
    }
}
