using Shared.Modelos;

namespace MeuDinheiro.Request;

public record DespesaRequest(int Id, string Descricao, double Valor, DateTime Data, int CategoriaId = 8);
