using ControleDespesas.Data;
using ControleDespesas.Models;
using ControleDespesas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDespesas.Controllers;

/// <summary>
/// Controller responsável por gerenciar a lógica da página de Dashboard.
/// </summary>
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor do DashboardController. Recebe uma instância do AppDbContext
    /// via injeção de dependência para interagir com o banco de dados.
    /// </summary>
    /// <param name="context">A instância do contexto do banco de dados.</param>
    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Action principal que calcula o resumo financeiro do mês corrente
    /// e exibe a página do Dashboard.
    /// </summary>
    /// <returns>Uma View contendo a ViewModel com os totais de receitas, despesas e saldo.</returns>
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var dataAtual = DateTime.Now;
        var inicioMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
        var fimMes = inicioMes.AddMonths(1).AddDays(-1);

        var receitaTotal = await CalculaReceitaTotal(inicioMes, fimMes);
        var despesaTotal = await CalculaDespesaTotal(inicioMes, fimMes);
        var saldoFinal = CalculaSaldoFinal(receitaTotal, despesaTotal);

        var agrupadas = await _context.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa &&
                        t.Data >= inicioMes && t.Data <= fimMes)
            .GroupBy(t => t.CategoriaId)
            .Select(g => new { CategoriaId = g.Key, Total = g.Sum(x => x.Valor) })
            .ToListAsync();

        var despesasPorCategoria = (from g in agrupadas
                                    join c in _context.Categorias on g.CategoriaId equals c.Id
                                    select new
                                    {
                                        Categoria = c.Nome,
                                        Total = g.Total
                                    })
                                    .OrderByDescending(x => x.Total)
                                    .ToList();

        var dashboard = new DashboardIndexViewModel
        {
            ReceitaTotal = receitaTotal,
            DespesaTotal = despesaTotal,
            SaldoFinal = saldoFinal,
            CategoriasDespesas = despesasPorCategoria.Select(x => x.Categoria).ToList(),
            ValoresDespesas = despesasPorCategoria.Select(x => x.Total).ToList()
        };

        return View("Index", dashboard);
    }

    /// <summary>
    /// Calcula a soma de todas as transações do tipo 'Receita' dentro de um período específico.
    /// </summary>
    /// <param name="inicioMes">A data inicial do período para o cálculo.</param>
    /// <param name="fimMes">A data final do período para o cálculo.</param>
    /// <returns>O valor decimal total das receitas no período.</returns>
    private async Task<decimal> CalculaReceitaTotal(DateTime inicioMes, DateTime fimMes)
    {
        var receitaTotal = await _context.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita && t.Data >= inicioMes && t.Data <= fimMes)
            .SumAsync(t => t.Valor);

        return receitaTotal;
    }

    /// <summary>
    /// Calcula a soma de todas as transações do tipo 'Despesa' dentro de um período específico.
    /// </summary>
    /// <param name="inicioMes">A data inicial do período para o cálculo.</param>
    /// <param name="fimMes">A data final do período para o cálculo.</param>
    /// <returns>O valor decimal total das despesas no período.</returns>
    private async Task<decimal> CalculaDespesaTotal(DateTime inicioMes, DateTime fimMes)
    {
        var despesaTotal = await _context.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa && t.Data >= inicioMes && t.Data <= fimMes)
            .SumAsync(t => t.Valor);

        return despesaTotal;
    }

    /// <summary>
    /// Calcula o saldo final subtraindo o total de despesas do total de receitas.
    /// </summary>
    /// <param name="receitaTotal">O valor total das receitas.</param>
    /// <param name="despesaTotal">O valor total das despesas.</param>
    /// <returns>O valor decimal do saldo final.</returns>
    private decimal CalculaSaldoFinal(decimal receitaTotal, decimal despesaTotal)
    {
        var saldoFinal = receitaTotal - despesaTotal;

        return saldoFinal;
    }
}