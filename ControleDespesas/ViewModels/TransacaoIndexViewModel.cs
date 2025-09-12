using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.ViewModels;

public class TransacaoIndexViewModel
{
    public int Id { get; set; }

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    [Display(Name = "Tipo")]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name = "Data")]
    public DateTime Data { get; set; }
}