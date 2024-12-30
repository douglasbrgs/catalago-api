using CatalogoApi.Models;
using CatalogoApi.Pagination;

namespace CatalogoApi.Repositories;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);

    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
