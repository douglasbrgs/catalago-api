﻿using CatalogoApi.Models;

namespace CatalogoApi.Repositories;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> GetCategorias();
    IEnumerable<Categoria> GetCategoriasProdutos();
    Categoria GetCategoria(int id);
    Categoria Create(Categoria categoria);
    Categoria Update(Categoria categoria);
    Categoria Delete(int id);
}
