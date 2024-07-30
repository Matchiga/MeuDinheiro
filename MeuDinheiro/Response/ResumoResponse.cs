namespace MeuDinheiro.Response;

public class ResumoResponse
{
    public double TotalReceitas { get; set; }
    public double TotalDespesas { get; set; }
    public double SaldoFinal { get; set; }
    public Dictionary<string, double> GastosPorCategoria { get; set; } = new();
}
