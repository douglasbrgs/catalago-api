using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories;

public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }
}
