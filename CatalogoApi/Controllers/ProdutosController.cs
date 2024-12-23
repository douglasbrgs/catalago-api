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

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("categoria/{id:int}")]
        public ActionResult<IEnumerable<Categoria>> GetProdutosCategoria(int id)
        {
            _logger.LogInformation("==================== GET api/categorias/produtos ====================");

            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            return Ok(produtos);
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("=================== GET api/produtos ==============================");

            var produtos = _uof.ProdutoRepository.GetAll();

            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"================== GET api/produtos/id {id} ======================");

            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                _logger.LogWarning($"============= Produto com id= {id} nao encontrado =================");
                return NotFound($"Produto com id= {id} nao encontrado");
            }

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            var produtoCriado = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produto);
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

            return Ok(produtoExcluido);
        }
    }
}