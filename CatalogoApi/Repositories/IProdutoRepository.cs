using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Filters;
using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Repositories;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltro);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
