// Importa o model Produto (estrutura dos dados)
using APIExercicio.Models;

// Importa o DbContext (conexão com o banco)
using APIExercicio.Data;

// Biblioteca padrão pra criar APIs (Controller, rotas, respostas HTTP)
using Microsoft.AspNetCore.Mvc;

// Permite usar métodos como FirstOrDefault() e ToList()
using System.Linq;



// Diz que essa classe é um controller de API
[ApiController]

// Define a rota base: /api/produto
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    // Representa a conexão com o banco de dados
    private readonly AppDbContext _context;

    // Construtor: o .NET injeta automaticamente o DbContext aqui
    public ProdutoController(AppDbContext context)
    {
        _context = context;
    }



    // =========================
    // 🔍 GET - LISTAR TODOS
    // =========================

    // Rota: GET /api/produto
    [HttpGet]
    public ActionResult<List<Produto>> Get()
    {
        // Busca todos os produtos no banco e retorna como lista
        return Ok(_context.Produtos.ToList());
    }



    // =========================
    // 🔍 GET POR ID
    // =========================

    // Rota: GET /api/produto/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        // Procura o primeiro produto com o ID informado
        var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

        // Se não encontrou, retorna erro 404
        if (produto == null)
        {
            return NotFound();
        }

        // Se encontrou, retorna o produto
        return Ok(produto);
    }



    // =========================
    // ➕ POST - CRIAR PRODUTO
    // =========================

    // Rota: POST /api/produto
    [HttpPost]
    public ActionResult<Produto> Post([FromBody] Produto produto)
    {
        // Adiciona o produto no banco (ainda não salva)
        _context.Produtos.Add(produto);

        // Salva de fato no banco de dados
        _context.SaveChanges();

        // Retorna 201 Created + rota para buscar o item criado
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }



    // =========================
    // ✏️ PUT - ATUALIZAR
    // =========================

    // Rota: PUT /api/produto/1
    [HttpPut("{id}")]
    public IActionResult Put(int id, Produto produto)
    {
        // Busca o produto existente no banco
        var existingProduto = _context.Produtos.FirstOrDefault(p => p.Id == id);

        // Se não existir, retorna 404
        if (existingProduto == null)
        {
            return NotFound();
        }

        // Atualiza os dados
        existingProduto.Nome = produto.Nome;
        existingProduto.Preco = produto.Preco;
        existingProduto.Estoque = produto.Estoque;

        // Salva as alterações no banco
        _context.SaveChanges();

        // Retorna sucesso sem conteúdo (204)
        return NoContent();
    }



    // =========================
    // ❌ DELETE - REMOVER
    // =========================

    // Rota: DELETE /api/produto/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Busca o produto no banco
        var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

        // Se não existir, retorna 404
        if (produto == null)
        {
            return NotFound();
        }

        // Remove o produto do banco
        _context.Produtos.Remove(produto);

        // Salva a remoção
        _context.SaveChanges();

        // Retorna sucesso sem conteúdo
        return NoContent();
    }
}