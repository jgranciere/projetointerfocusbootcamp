using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendendoAPI.Models
{
    [Table("pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [Column("data_entrega")]
        public DateTime? DataEntrega { get; set; } 

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } 

        [Required]
        [Column("total_pedido")]
        public decimal TotalPedido { get; set; }

        [Required]
        [Column("endereco")]
        [MaxLength(200)]
        public string Endereco { get; set; }

        [Required]
        [Column("nome_cliente")]
        [MaxLength(100)]
        public string NomeCliente { get; set; }

        public ICollection<ItemPedido> Itens { get; set; } 

        public Pedido()
        {
            DataCriacao = DateTime.UtcNow; 
            Status = "Pendente"; 
            Itens = new List<ItemPedido>();
        }
    }
}