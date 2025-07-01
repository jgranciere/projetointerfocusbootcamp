
namespace AprendendoAPI.ViewModel
{
    public class ItemPedidoResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public ProdutoResponseDto Produto { get; set; } 
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotalItem { get; set; }
    }
}