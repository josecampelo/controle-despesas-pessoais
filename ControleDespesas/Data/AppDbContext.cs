using ControleDespesas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDespesas.Data;

public class AppDbContext : DbContext
{
    /// <summary>
    /// Construtor do nosso DbContext. Ele recebe as opções de configuração
    /// do banco de dados (como a string de conexão) que serão injetadas
    /// pelo sistema de injeção de dependência do .NET.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Mapeia a entidade 'Transacao' para uma tabela no banco de dados.
    /// </summary>
    public DbSet<Transacao> Transacoes { get; set; }
}