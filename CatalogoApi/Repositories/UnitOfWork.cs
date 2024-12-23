using CatalogoApi.Context;

namespace CatalogoApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private CategoriaRepository? categoriaRepo;
    private ProdutoRepository? produtoRepo;
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            if (categoriaRepo is null)
            {
                categoriaRepo = new CategoriaRepository(_context);
            }

            return categoriaRepo;
        }
    }

    public IProdutoRepository ProdutoRepository
    {
        get
        {
            if (produtoRepo is null)
            {
                produtoRepo = new ProdutoRepository(_context);
            }

            return produtoRepo;
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
