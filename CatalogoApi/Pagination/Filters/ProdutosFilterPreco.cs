using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Pagination.Filters;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCriterio { get; set; }
}
