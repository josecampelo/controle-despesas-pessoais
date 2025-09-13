using ControleDespesas.Models;
using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.ViewModels;

/// <summary>
/// ViewModel utilizada para exibir os dados de uma única transação
/// na tela de listagem (Index).
/// </summary>
public class TransacaoIndexViewModel
{
    /// <summary>
    /// Identificador único da transação.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Breve descrição da transação.
    /// </summary>
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// O valor monetário da transação.
    /// </summary>
    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    /// <summary>
    /// O tipo da transação (Receita ou Despesa).
    /// </summary>
    [Display(Name = "Tipo")]
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// A data em que a transação ocorreu.
    /// </summary>
    [Display(Name = "Data")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime Data { get; set; }
}