using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AprendendoAPI.Infraestrutura;

namespace AprendendoAPI.Models
{
    [Table("produtos_mais_pedidos")]
    public class ProdutoMaisPedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("produto_id")]
        public int ProdutoId { get; set; }

        public Produto Produto { get; set; }

        [Column("categoria")]
        public string? Categoria { get; set; }
    }
}
