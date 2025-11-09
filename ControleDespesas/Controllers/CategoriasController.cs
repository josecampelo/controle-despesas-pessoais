using ControleDespesas.Data;
using ControleDespesas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDespesas.Controllers;

public class CategoriasController : Controller
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna categorias (opcionalmente filtradas por tipo) em JSON.
    /// Ex.: GET /Categorias?tipo=Despesa
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListJson([FromQuery] TipoTransacao? tipo)
    {
        var query = _context.Categorias.AsQueryable();

        if (tipo.HasValue)
            query = query.Where(c => c.Tipo == tipo.Value);

        var categorias = await query
            .OrderBy(c => c.Nome)
            .Select(c => new { c.Id, c.Nome })
            .ToListAsync();

        return Json(categorias);
    }

    /// <summary>
    /// Cria uma nova categoria. Retorna JSON com Id e Nome.
    /// Pensado para uso via AJAX.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJson([FromForm] string nome, [FromForm] TipoTransacao tipo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return BadRequest("O campo Nome é obrigatório.");

        var exists = await _context.Categorias
            .AnyAsync(c => c.Nome == nome && c.Tipo == tipo);

        if (exists)
            return Conflict("Já existe uma categoria com esse nome para esse tipo.");

        var categoria = new Categoria { Nome = nome.Trim(), Tipo = tipo };
        _context.Categorias.Add(categoria);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Não foi possível salvar: categoria duplicada.");
        }

        return Json(new { categoria.Id, categoria.Nome });
    }
}