using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories;

public class ProdutoRepository : GenericRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}
