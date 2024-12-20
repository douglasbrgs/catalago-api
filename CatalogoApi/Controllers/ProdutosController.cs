﻿using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("=================== GET api/produtos ==============================");

            var produtos = _repository.GetProdutos();

            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public IActionResult GetAsync(int id)
        {
            _logger.LogInformation($"================== GET api/produtos/id {id} ======================");

            var produto = _repository.GetProduto(id);

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

            var produtoCriado = _repository.Create(produto);

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

            _repository.Update(produto);

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var produto = _repository.GetProduto(id);

            if (produto is null)
            {
                _logger.LogWarning($"============= Produto com id= {id} nao encontrado =============");
                return NotFound($"Produto com id= {id} nao encontrado");
            }

            var produtoExcluido = _repository.Delete(id);

            return Ok(produtoExcluido);
        }
    }
}