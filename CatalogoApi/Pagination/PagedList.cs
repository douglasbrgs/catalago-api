namespace CatalogoApi.Pagination;

/// <summary> Classe genérica para gerenciar a paginação de dados</summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T> : List<T> where T : class
{
    /// <summary> Numero da pagina atual.</summary>
    public int CurrentPage { get; private set; }

    /// <summary> Numero total de paginas.</summary>
    public int TotalPages { get; private set; }

    /// <summary> Numero de itens por pagina.</summary>
    public int PageSize { get; private set; }

    /// <summary> Total de itens na fonte de dados.</summary>
    public int TotalCount { get; private set; }

    /// <summary> Indica se ha uma pagina anterior.</summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary> Indica se ha uma proxima pagina.</summary>
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();

        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
