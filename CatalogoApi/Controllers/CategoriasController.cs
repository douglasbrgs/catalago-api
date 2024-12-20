using CatalogoApi.Filters;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private ICategoriaRepository _repository;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("=================== GET api/categorias ==============================");

            var categorias = _repository.GetAll();

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"================== GET api/categorias/id {id} ======================");

            var categoria = _repository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"============= Categoria com id= {id} nao encontrada =================");
                return NotFound($"Categoria com id= {id} nao encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public IActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("==================== Dados inválidos ==============================");
                return BadRequest("Dados inválidos");
            }

            _repository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var categoria = _repository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"============= Categoria com id= {id} nao encontrada =============");
                return NotFound($"Categoria com id= {id} nao encontrada");
            }

            var categoriaExcluida = _repository.Delete(categoria);

            return Ok(categoriaExcluida);
        }
    }
}