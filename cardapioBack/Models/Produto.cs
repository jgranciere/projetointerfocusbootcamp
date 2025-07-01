using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendendoAPI.Models
{
    [Table("produtos")]
    public class Produto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Column("descricao")]
        [MaxLength(100)]
        public string Descricao { get; set; }

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("imagemurl")]
        public string? ImagemUrl { get; set; }

        [Column("categoria")]
        [MaxLength(50)]
        public string? Categoria { get; set; }

        [Required]
        [Column("quantidade_maxima")]
        public int QuantidadeMaxima { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; }

        public Produto(string nome, decimal preco, string descricao, string imagemUrl, int quantidadeMaxima, string status)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            ImagemUrl = imagemUrl;
            QuantidadeMaxima = quantidadeMaxima;
            Status = status;
        }

        
    }
}
