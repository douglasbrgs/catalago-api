using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Pagination.Filters;

public class CategoriasFiltroNome : QueryStringParameters
{
    public string? Nome { get; set; }
}
