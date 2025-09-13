using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.Models;

/// <summary>
/// Define os tipos de transações financeiras possíveis na aplicação.
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Representa uma entrada de dinheiro (ex: salário, vendas).
    /// </summary>
    [Display(Name = "Receita")]
    Receita = 1,

    /// <summary>
    /// Representa uma saída de dinheiro (ex: compras, contas).
    /// </summary>
    [Display(Name = "Despesa")]
    Despesa = 2
}