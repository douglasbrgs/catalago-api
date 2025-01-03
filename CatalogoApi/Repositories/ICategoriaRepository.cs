using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Filters;
using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
    PagedList<Categoria> GetCategoriasPorNome(CategoriasFiltroNome categoriasFiltro);
}
