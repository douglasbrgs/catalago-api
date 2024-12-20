using CatalogoApi.Models;

namespace CatalogoApi.Repositories;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
