namespace AprendendoAPI.ViewModel
{
    public class ProdutoViewModel
    {
        public string Nome { get; set; }
        public Decimal Preco { get; set; } 
        public string Descricao { get; set; }
        public IFormFile? foto { get; set; }
    }
}
