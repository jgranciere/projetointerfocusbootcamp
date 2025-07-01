namespace AprendendoAPI.ViewModel
{
    public class PedidoViewModel
    {
        public string NomeCliente { get; set; }
        public string Endereco { get; set; }
        public List<ItemPedidoViewModel> Itens { get; set; }
    }
}
