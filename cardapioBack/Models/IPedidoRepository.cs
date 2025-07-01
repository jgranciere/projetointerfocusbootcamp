using AprendendoAPI.ViewModel;
using System.Collections.Generic;

namespace AprendendoAPI.Models
{
    public interface IPedidoRepository
    {
        void Add(Pedido pedido);
        List<PedidoResponseDto> GetAll();
        PedidoResponseDto GetById(int id);
        void Update(Pedido pedido);
        void CancelarPedido(int id);
        void FinalizarPedido(int id);
        decimal GetMonthlyTotalConcludedOrders(int year, int month);
    }
}