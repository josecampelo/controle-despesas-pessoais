using ControleDespesas.Models;
using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.ViewModels;

public class TransacaoViewModel
{
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
    [Display(Name = "Tipo")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "O campo Data é obrigatório.")]
    [Display(Name = "Data")]
    public DateTime Data { get; set; }
}