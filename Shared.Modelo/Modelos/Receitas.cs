namespace Shared.Modelos;

public class Receitas
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public double Valor { get; set; }
    public DateTime Data { get; set; }
    public Receitas(int id, string descricao, double valor, DateTime data)
    {
        Id = id;
        Descricao = descricao;
        Valor = valor;
        Data = data;
    }
}
