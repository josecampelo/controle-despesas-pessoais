using ControleDespesas.Data;
using ControleDespesas.Models;
using ControleDespesas.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        var todasAsTransacoes = await _context.Transacoes.ToListAsync();
        
        return View("Index", todasAsTransacoes);
    }

    /// <summary>
    /// Action para exibir o formulário de criação de uma nova transação.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Action que recebe os dados do formulário e cria uma nova transação no banco.
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
                    Data = model.Data
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

        return View("Create", model);
    }
}