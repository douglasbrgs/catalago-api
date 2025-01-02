using AutoMapper;
using CatalogoApi.DTOs;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Pagination.Filters;
using CatalogoApi.Pagination.Parameters;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CatalogoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<ProdutosController> _logger;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("categoria/{id:int}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            _logger.LogInformation("==================== GET api/categorias/produtos ====================");

            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            var produtoDTOs = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtoDTOs);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            _logger.LogInformation("=================== GET api/produtos ==============================");

            var produtos = _uof.ProdutoRepository.GetAll();

            var produtoDTOs = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtoDTOs);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            return ObterProdutos(produtos);
        }

        [HttpGet("filter")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosFiltroPreco produtosFiltro)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltro);

            return ObterProdutos(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            _logger.LogInformation($"================== GET api/produtos/id {id} ======================");

            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                _logger.LogWarning($"============= Produto com id= {id} nao encontrado =================");
                return NotFound($"Produto com id= {id} nao encontrado");
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoCriado = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            var produtoCriadoDTO = _mapper.Map<ProdutoDTO>(produtoCriado);

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriadoDTO.ProdutoId }, produtoCriadoDTO);
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,
            JsonPatchDocument<ProdutoDTOUpdateRequest> patchDocument)
        {
            if (patchDocument is null || id <= 0)
            {
                return BadRequest();
            }

            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound();
            }

            // Mapeia Produto para DTO Request
            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            // Aplica alteracoes definidas no JSON Patch
            patchDocument.ApplyTo(produtoUpdateRequest, ModelState);

            // Valida os dados de entrada
            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            // Mapeia DTO Request para Produto
            _mapper.Map(produtoUpdateRequest, produto);

            // Atualiza Produto no repositorio
            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            // Mapeia para DTO Response
            var produtoAlteradoDTO = _mapper.Map<ProdutoDTOUpdateResponse>(produto);

            return Ok(produtoAlteradoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizadoDTO);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                _logger.LogWarning($"============= Produto com id= {id} nao encontrado =============");
                return NotFound($"Produto com id= {id} nao encontrado");
            }

            var produtoExcluido = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoExcluidoDTO = _mapper.Map<ProdutoDTO>(produtoExcluido);

            return Ok(produtoExcluidoDTO);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtoDTOs = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtoDTOs);
        }

    }
}