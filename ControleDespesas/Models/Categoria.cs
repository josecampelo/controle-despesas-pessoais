using System.ComponentModel.DataAnnotations;

namespace ControleDespesas.Models
{
    /// <summary>
    /// Representa uma categoria financeira dentro do sistema de controle de despesas pessoais.
    /// Cada categoria é classificada como uma <see cref="TipoTransacao"/> (Receita ou Despesa)
    /// e pode ser associada a uma ou mais transações.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Identificador único da categoria.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria.
        /// Exemplo: "Alimentação", "Salário", "Transporte".
        /// </summary>
        /// <remarks>
        /// Campo obrigatório e limitado a 60 caracteres para evitar nomes excessivamente longos.
        /// </remarks>
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(60, ErrorMessage = "O nome da categoria deve ter no máximo 60 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de transação associada à categoria.
        /// Pode ser <see cref="TipoTransacao.Receita"/> ou <see cref="TipoTransacao.Despesa"/>.
        /// </summary>
        /// <remarks>
        /// Essa propriedade é obrigatória e garante que cada categoria
        /// esteja vinculada a um tipo de movimentação financeira coerente.
        /// </remarks>
        [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
        public TipoTransacao Tipo { get; set; }
    }
}