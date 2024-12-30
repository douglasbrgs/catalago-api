using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
}
