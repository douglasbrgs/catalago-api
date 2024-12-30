using CatalogoApi.Models;
using CatalogoApi.Pagination;

namespace CatalogoApi.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
}
