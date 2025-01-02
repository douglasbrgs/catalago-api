using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Filters;
using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Repositories;

public class ProdutoRepository : GenericRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltro)
    {
        var produtos = GetAll().AsQueryable();

        if (produtosFiltro.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltro.PrecoCriterio))
        {
            if ("maior".Equals(produtosFiltro.PrecoCriterio, StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltro.Preco).OrderBy(p => p.Preco);
            }
            if ("menor".Equals(produtosFiltro.PrecoCriterio, StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltro.Preco).OrderBy(p => p.Preco);
            }
            if ("igual".Equals(produtosFiltro.PrecoCriterio, StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltro.Preco).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltro.PageNumber, produtosFiltro.PageSize);

        return produtosFiltrados;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}
