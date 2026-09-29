using Microsoft.EntityFrameworkCore;
using Produtos.Api.Models;

namespace Produtos.Api.Data;

public class AppDbContext : DbContext
{
   public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Produto> Produtos {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Produto>()
        .Property(produto => produto.Preco)
        .HasPrecision(18, 2);
}
}