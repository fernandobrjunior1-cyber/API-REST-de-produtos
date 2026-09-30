using Microsoft.AspNetCore.Mvc; // Recursos para criar controllers
using Microsoft.EntityFrameworkCore; // Métodos de acesso ao banco
using Produtos.Api.Data; // Nosso contexto do banco
using Produtos.Api.Models; // Classe Produto

namespace Produtos.Api.Controllers;

[ApiController] // Ativa comportamentos e validações de API
[Route("produtos")] // Define a rota /produtos
public class ProdutosController : ControllerBase
{
    // Guarda o contexto para acessar o banco
    private readonly AppDbContext _context;

    // O .NET fornece o contexto por injeção de dependência
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet] // GET /produtos ou /produtos?nome=teclado
public async Task<ActionResult<List<Produto>>> Listar(
    [FromQuery] string? nome)
{
    // Prepara a consulta, sem executá-la ainda
    var consulta = _context.Produtos.AsQueryable();

    // Aplica o filtro se um nome foi informado
    if (!string.IsNullOrWhiteSpace(nome))
    {
        var termo = nome.Trim();

        consulta = consulta.Where(produto =>
            produto.Nome.Contains(termo));
    }

    // Executa a consulta no banco
    var produtos = await consulta.ToListAsync();

    return Ok(produtos);
}

    [HttpPost]
    public async Task<ActionResult<Produto>> Criar(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return StatusCode(201, produto);
    }

   [HttpGet("{id:int}")] // Exemplo: GET /produtos/1
public async Task<ActionResult<Produto>> Consultar(int id)
{
    // Busca o produto pela chave primária (Id)
    var produto = await _context.Produtos.FindAsync(id);

    // Retorna 404 se não encontrar
    if (produto == null)
    {
        return NotFound();
    }

    // Retorna 200 com o produto em JSON
    return Ok(produto);
   }
     [HttpPut("{id:int}")] // Exemplo: PUT /produtos/1
public async Task<IActionResult> Atualizar(int id, Produto dados)
{
    // Busca o produto no banco
    var produto = await _context.Produtos.FindAsync(id);

    // Retorna 404 se não encontrar
    if (produto == null)
    {
        return NotFound();
    }

    // Substitui os dados editáveis do produto
    produto.Nome = dados.Nome;
    produto.Preco = dados.Preco;

    // Salva as alterações no SQL Server
    await _context.SaveChangesAsync();

    // Retorna 204: sucesso, sem corpo na resposta
    return NoContent();
    }

    [HttpDelete("{id:int}")] // Exemplo: DELETE /produtos/1
public async Task<IActionResult> Excluir(int id)
{
    // Busca o produto no banco
    var produto = await _context.Produtos.FindAsync(id);

    // Retorna 404 se não encontrar
    if (produto == null)
    {
        return NotFound();
    }

    // Marca o produto para exclusão
    _context.Produtos.Remove(produto);

    // Executa a exclusão no SQL Server
    await _context.SaveChangesAsync();

    // Retorna 204: sucesso, sem corpo na resposta
    return NoContent();
}

}