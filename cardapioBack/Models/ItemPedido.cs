using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendendoAPI.Models
{
    [Table("itens_pedido")]
    public class ItemPedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("produto_id")]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; } 

        [Required]
        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Required]
        [Column("preco_unitario")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [Column("preco_total_item")]
        public decimal PrecoTotalItem { get; set; }

        [Required]
        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } 
    }
}