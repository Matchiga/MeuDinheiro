using Shared.Modelos;
using System.Runtime.ConstrainedExecution;
namespace MeuDinheiro.Request;

public record DespesaRequestEdit(int Id, string Descricao, double Valor, DateTime Data, int CategoriaId) : DespesaRequest(Id, Descricao, Valor, Data, CategoriaId);