using System;
using System.Collections.Generic;

namespace AprendendoAPI.ViewModel
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Status { get; set; }
        public decimal TotalPedido { get; set; }
        public string Endereco { get; set; }
        public string NomeCliente { get; set; }
        public List<ItemPedidoResponseDto> Itens { get; set; } 
    }
}