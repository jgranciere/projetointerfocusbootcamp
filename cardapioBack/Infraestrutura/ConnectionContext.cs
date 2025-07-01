using AprendendoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AprendendoAPI.Infraestrutura
{
    public class ConnectionContext : DbContext
    {

        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options)
        {

        }
        //esqueleto base para conexão com DB
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ProdutoMaisPedido> ProdutosMaisPedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql(
        //        "Host=localhost;" +
        //        "Port=5432;Database=teste_produtos;" +
        //        "Username=postgres;" +
        //        "Password=1234;"+
        //        "Search Path=public");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany()
                .HasForeignKey(ip => ip.ProdutoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(ip => ip.PedidoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
