﻿using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Parameters;

namespace CatalogoApi.Repositories;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);

    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
