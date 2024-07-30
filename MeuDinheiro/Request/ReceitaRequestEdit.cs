namespace MeuDinheiro.Request;

public record ReceitaRequestEdit(int Id, string Descricao, double Valor, DateTime Data) : ReceitaRequest(Id, Descricao, Valor, Data);