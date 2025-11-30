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
    /// Action para exibir a lista de transações com filtros de Mês, Ano, Tipo e Descrição.
    /// </summary>
    /// <param name="mes">O mês para filtrar (1 a 12).</param>
    /// <param name="ano">O ano para filtrar.</param>
    /// <param name="tipo">O tipo de transação (Receita/Despesa).</param>
    /// <param name="descricao">Texto para busca parcial na descrição.</param>
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int? mes, int? ano, TipoTransacao? tipo, string? descricao)
    {
        var dataAtual = DateTime.Now;
        int mesFiltro = mes ?? dataAtual.Month;
        int anoFiltro = ano ?? dataAtual.Year;
        var query = _context.Transacoes.AsQueryable();

        query = query.Where(t => t.Data.Month == mesFiltro && t.Data.Year == anoFiltro);

        if (tipo.HasValue)
        {
            query = query.Where(t => t.Tipo == tipo.Value);
        }

        if (!string.IsNullOrWhiteSpace(descricao))
        {
            query = query.Where(t => t.Descricao.Contains(descricao));
        }

        var model = await query
            .OrderByDescending(transacao => transacao.Data)
            .ThenByDescending(transacao => transacao.Id)
            .Select(transacao => new TransacaoIndexViewModel
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                Data = transacao.Data
            })
            .ToListAsync();

        CarregarViewBagFiltros(mesFiltro, anoFiltro);

        ViewBag.FiltroTipo = tipo;
        ViewBag.FiltroDescricao = descricao;

        return View("Index", model);
    }

    /// <summary>
    /// Método auxiliar para preencher as listas de seleção (Meses e Anos) na interface de filtro.
    /// </summary>
    private void CarregarViewBagFiltros(int mesSelecionado, int anoSelecionado)
    {
        var meses = new List<SelectListItem>();
        for (int i = 1; i <= 12; i++)
        {
            var nomeMes = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
            nomeMes = char.ToUpper(nomeMes[0]) + nomeMes.Substring(1);

            meses.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = nomeMes,
                Selected = i == mesSelecionado
            });
        }
        ViewBag.Meses = meses;

        var anoAtual = DateTime.Now.Year;
        var anos = new List<SelectListItem>();
        for (int i = anoAtual - 5; i <= anoAtual + 1; i++)
        {
            anos.Add(new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString(),
                Selected = i == anoSelecionado
            });
        }
        ViewBag.Anos = anos;
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