﻿namespace AprendendoAPI.ViewModel
{
    public class ProdutoViewModel
    {
        public string Nome { get; set; }
        public Decimal Preco { get; set; } 
        public string Descricao { get; set; }
        public IFormFile? foto { get; set; }
        public string? Categoria { get; set; }
        public int QuantidadeMaxima { get; set; }
        public string Status { get; set; }
    }
}
