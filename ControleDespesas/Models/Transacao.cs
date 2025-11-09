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
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// A data e hora em que a transação ocorreu.
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Identificador da categoria associada a esta transação.
    /// Representa a relação com a tabela de Categorias no banco de dados.
    /// </summary>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Categoria associada a esta transação.
    /// Permite o acesso direto aos dados da categoria (navegação).
    /// </summary>
    public Categoria? Categoria { get; set; }
}