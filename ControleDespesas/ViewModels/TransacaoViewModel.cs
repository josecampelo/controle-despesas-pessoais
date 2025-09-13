using ControleDespesas.Models;
using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.ViewModels;

/// <summary>
/// ViewModel utilizada para as operações de criação e edição de uma Transação.
/// Contém as validações de dados necessárias para o formulário.
/// </summary>
public class TransacaoViewModel
{
    /// <summary>
    /// Breve descrição da transação.
    /// </summary>
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// O valor monetário da transação. Deve ser maior que zero.
    /// </summary>
    [Required(ErrorMessage = "O campo Valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    /// <summary>
    /// O tipo da transação (Receita ou Despesa).
    /// </summary>
    [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
    [Display(Name = "Tipo")]
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// A data em que a transação ocorreu.
    /// </summary>
    [Required(ErrorMessage = "O campo Data é obrigatório.")]
    [Display(Name = "Data")]
    public DateTime Data { get; set; }
}