namespace MeuDinheiro.Request;

public record ReceitaRequest(int Id, string Descricao, double Valor, DateTime Data);