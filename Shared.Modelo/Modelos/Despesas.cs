using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Modelos;

public class Despesas
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public double Valor { get; set; }
    public int CategoriaId { get; set; }
    [ForeignKey("CategoriaId")]
    public Categoria? Categoria { get; set; }
    public Despesas(int id, string descricao, double valor, DateTime data)
    {
        Id = id;
        Descricao = descricao;
        Valor = valor;
        Data = data;
    }
}
