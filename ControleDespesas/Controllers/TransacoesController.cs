using ControleDespesas.Data;
using ControleDespesas.Models;
using ControleDespesas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ControleDespesas.Controllers;

public class TransacoesController : Controller
{
    private readonly AppDbContext _context;

    public TransacoesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Action para exibir a lista de todas as transações.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = await _context.Transacoes
            .OrderByDescending(transacao => transacao.Id)
            .Select(transacao => new TransacaoIndexViewModel
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                Data = transacao.Data
            })
            .ToListAsync();

        return View("Index", model);
    }

    /// <summary>
    /// Action para exibir o formulário de criação de uma nova transação.
    /// Agora também carrega a lista de categorias disponíveis.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categorias = await ObterCategoriasAsync();

        var viewModel = new TransacaoViewModel
        {
            Data = DateTime.Now,
            Categorias = categorias
        };

        return View(viewModel);
    }

    /// <summary>
    /// Action que recebe os dados do formulário e cria uma nova transação no banco.
    /// Agora inclui o vínculo com a categoria selecionada.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(TransacaoViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var transacao = new Transacao
                {
                    Descricao = model.Descricao,
                    Valor = model.Valor,
                    Tipo = model.Tipo,
                    Data = model.Data,
                    CategoriaId = model.CategoriaId
                };

                _context.Add(transacao);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Não foi possível salvar os dados.");
        }

        model.Categorias = await ObterCategoriasAsync();

        return View("Create", model);
    }

    /// <summary>
    /// Carrega todas as categorias do banco e retorna uma lista formatada para exibição em um dropdown.
    /// </summary>
    private async Task<List<SelectListItem>> ObterCategoriasAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Nome)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Nome} ({c.Tipo})"
            })
            .ToListAsync();
    }
}