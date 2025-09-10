using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDespesas.Models;

/// <summary>
/// Representa uma transação financeira, que pode ser uma receita ou uma despesa.
/// Esta classe é a entidade principal da aplicação.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação (Chave Primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Breve descrição da transação. Ex: "Almoço", "Salário do mês".
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// O valor monetário da transação.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Define o tipo da transação. Deve conter "Receita" ou "Despesa".
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// A data e hora em que a transação ocorreu.
    /// </summary>
    public DateTime Data { get; set; }
}