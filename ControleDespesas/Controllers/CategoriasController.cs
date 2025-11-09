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
    /// Exibe a lista de todas as categorias cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Tipo)
            .ThenBy(c => c.Nome)
            .ToListAsync();

        return View("Index", categorias);
    }

    /// <summary>
    /// Exibe o formulário de criação de uma nova categoria.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Recebe os dados do formulário e cria uma nova categoria.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync([Bind("Nome,Tipo")] Categoria model)
    {
        model.Nome = model.Nome?.Trim() ?? string.Empty;

        if (!ModelState.IsValid)
            return View("Create", model);

        var existe = await _context.Categorias
            .AnyAsync(c => c.Nome == model.Nome && c.Tipo == model.Tipo);

        if (existe)
        {
            ModelState.AddModelError(nameof(Categoria.Nome),
                "Já existe uma categoria com esse nome para esse tipo.");
            return View("Create", model);
        }

        try
        {
            _context.Categorias.Add(model);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Categoria criada com sucesso.";
            return RedirectToAction("Index");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Não foi possível salvar os dados.");
            return View("Create", model);
        }
    }

    /// <summary>
    /// Exibe o formulário de edição de uma categoria existente.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    /// <summary>
    /// Recebe os dados do formulário e atualiza a categoria no banco de dados.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(int id, [Bind("Id,Nome,Tipo")] Categoria model)
    {
        if (id != model.Id)
            return NotFound();

        model.Nome = model.Nome?.Trim() ?? string.Empty;

        if (!ModelState.IsValid)
            return View("Edit", model);

        var duplicada = await _context.Categorias
            .AnyAsync(c => c.Id != model.Id && c.Nome == model.Nome && c.Tipo == model.Tipo);

        if (duplicada)
        {
            ModelState.AddModelError(nameof(Categoria.Nome),
                "Já existe uma categoria com esse nome e tipo.");
            return View("Edit", model);
        }

        try
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Categoria atualizada com sucesso.";
            return RedirectToAction("Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categorias.Any(c => c.Id == model.Id))
                return NotFound();

            throw;
        }
    }

    /// <summary>
    /// Exibe a página de confirmação para excluir uma categoria.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var categoria = await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    /// <summary>
    /// Confirma a exclusão da categoria, se não houver transações vinculadas.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
            return NotFound();

        bool temTransacoes = await _context.Transacoes
            .AnyAsync(t => t.CategoriaId == id);

        if (temTransacoes)
        {
            TempData["Erro"] = "Não é possível excluir uma categoria associada a transações existentes.";
            return RedirectToAction("Index");
        }

        try
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Categoria excluída com sucesso.";
        }
        catch (DbUpdateException)
        {
            TempData["Erro"] = "Ocorreu um erro ao tentar excluir a categoria.";
        }

        return RedirectToAction("Index");
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