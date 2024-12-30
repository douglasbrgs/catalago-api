using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Parameters;

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
}
