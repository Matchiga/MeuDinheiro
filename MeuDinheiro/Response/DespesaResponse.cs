using Shared.Modelos;

namespace MeuDinheiro.Response;

public record DespesaResponse(int Id, string Descricao, double Valor, DateTime Data, int CategoriaId, string NomeCategoria);