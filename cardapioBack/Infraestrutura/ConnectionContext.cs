using AprendendoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AprendendoAPI.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        //esqueleto base para conexão com DB
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ProdutoMaisPedido> ProdutosMaisPedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                "Host=localhost;" +
                "Port=5432;Database=teste_produtos;" +
                "Username=postgres;" +
                "Password=1234;"+
                "Search Path=public");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");  

            base.OnModelCreating(modelBuilder);
        }
    }
}
