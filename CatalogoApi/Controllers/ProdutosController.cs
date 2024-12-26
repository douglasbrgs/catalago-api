using AutoMapper;
using CatalogoApi.DTOs;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

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
    }
}