﻿namespace AprendendoAPI.ViewModel
{
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
        public string Categoria { get; set; }
        public int QuantidadeMaxima { get; set; }
        public string Status { get; set; }
       
    }
}