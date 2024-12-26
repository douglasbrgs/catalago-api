using AutoMapper;
using CatalogoApi.Models;

namespace CatalogoApi.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
    public ProdutoDTOMappingProfile()
    {
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
    }
}
