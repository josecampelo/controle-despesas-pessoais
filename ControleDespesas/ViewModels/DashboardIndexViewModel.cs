using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.ViewModels;

/// <summary>
/// ViewModel utilizada para transportar e exibir os dados de resumo financeiro
/// na página do Dashboard.
/// </summary>
public class DashboardIndexViewModel
{
    /// <summary>
    /// Representa a soma total das receitas calculadas para o período exibido no dashboard.
    /// </summary>
    [Display(Name = "Receita Total")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal ReceitaTotal { get; set; }

    /// <summary>
    /// Representa a soma total das despesas calculadas para o período exibido no dashboard.
    /// </summary>
    [Display(Name = "Despesa Total")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal DespesaTotal { get; set; }

    /// <summary>
    /// Representa o saldo final do período, calculado como (ReceitaTotal - DespesaTotal).
    /// </summary>
    [Display(Name = "Saldo Final")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal SaldoFinal { get; set; }

    /// <summary>
    /// Rótulos do gráfico de pizza (nomes das categorias de despesa).
    /// </summary>
    public List<string> CategoriasDespesas { get; set; } = new();

    /// <summary>
    /// Valores do gráfico de pizza (soma das despesas por categoria).
    /// </summary>
    public List<decimal> ValoresDespesas { get; set; } = new();
}