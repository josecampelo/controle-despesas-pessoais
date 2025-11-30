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

    /// <summary>
    /// Action para exibir o formulário de edição de uma transação existente.
    /// Busca a transação pelo ID e preenche a ViewModel.
    /// </summary>
    /// <param name="id">Identificador único da transação a ser editada.</param>
    /// <returns>A View de edição com os dados carregados ou NotFound se não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
            return NotFound();

        var viewModel = new TransacaoViewModel
        {
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            Data = transacao.Data,
            CategoriaId = transacao.CategoriaId,
            Categorias = await ObterCategoriasAsync()
        };

        ViewBag.Id = transacao.Id;

        return View(viewModel);
    }

    /// <summary>
    /// Action que recebe os dados editados do formulário e atualiza a transação no banco de dados.
    /// </summary>
    /// <param name="id">O ID da transação que está sendo atualizada.</param>
    /// <param name="model">A ViewModel com os novos dados preenchidos pelo usuário.</param>
    /// <returns>Redireciona para Index em caso de sucesso, ou retorna a View com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TransacaoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categorias = await ObterCategoriasAsync();
            ViewBag.Id = id;
            return View(model);
        }

        try
        {
            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao == null)
                return NotFound();

            transacao.Descricao = model.Descricao;
            transacao.Valor = model.Valor;
            transacao.Tipo = model.Tipo;
            transacao.Data = model.Data;
            transacao.CategoriaId = model.CategoriaId;

            _context.Update(transacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Transacoes.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }
    }

    /// <summary>
    /// Action para exibir a página de confirmação de exclusão de uma transação.
    /// Carrega os detalhes da transação (incluindo a categoria) para o usuário conferir.
    /// </summary>
    /// <param name="id">Identificador único da transação a ser excluída.</param>
    /// <returns>A View de confirmação de exclusão.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var transacao = await _context.Transacoes
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (transacao == null)
            return NotFound();

        return View(transacao);
    }

    /// <summary>
    /// Action que efetivamente remove a transação do banco de dados após a confirmação.
    /// </summary>
    /// <param name="id">O ID da transação a ser removida.</param>
    /// <returns>Redireciona para a lista de transações (Index).</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);

        if (transacao != null)
        {
            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}