using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Filters;
using CatalogoApi.Pagination.Parameters;
using System.Linq;

namespace CatalogoApi.Repositories;

public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasPorNome(CategoriasFiltroNome categoriasFiltro)
    {
        var categorias = GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(categoriasFiltro.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltro.Nome, StringComparison.InvariantCultureIgnoreCase));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltro.PageNumber, categoriasFiltro.PageSize);

        return categoriasFiltradas;
    }
}
