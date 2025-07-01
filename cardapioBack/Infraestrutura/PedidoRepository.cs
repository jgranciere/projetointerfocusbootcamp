using AprendendoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AprendendoAPI.ViewModel;

namespace AprendendoAPI.Infraestrutura
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ConnectionContext _context; 

        public PedidoRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Pedido pedido)
        {
            
            foreach (var item in pedido.Itens)
            {
                var produto = _context.Produto.Find(item.ProdutoId);

                if (produto == null)
                {
                    throw new ArgumentException($"Produto com ID {item.ProdutoId} não encontrado.");
                }

                if (produto.Status == "Indisponivel")
                {
                    throw new InvalidOperationException($"Produto '{produto.Nome}' está indisponível.");
                }

                if (item.Quantidade <= 0)
                {
                    throw new ArgumentException($"Quantidade inválida para o produto '{produto.Nome}'.");
                }

                if (item.Quantidade > produto.QuantidadeMaxima)
                {
                    throw new InvalidOperationException($"Quantidade solicitada ({item.Quantidade}) para '{produto.Nome}' excede a quantidade máxima permitida ({produto.QuantidadeMaxima}).");
                }

                
                produto.QuantidadeMaxima -= item.Quantidade;

                if (produto.QuantidadeMaxima <= 0)
                {
                    produto.Status = "Indisponivel"; 
                }
                _context.Produto.Update(produto); 
            }

            
            foreach (var item in pedido.Itens)
            {
                var produto = _context.Produto.Find(item.ProdutoId);
                item.PrecoUnitario = produto.Preco; 
                item.PrecoTotalItem = item.Quantidade * item.PrecoUnitario;
            }
            pedido.TotalPedido = pedido.Itens.Sum(item => item.PrecoTotalItem);

            _context.Pedidos.Add(pedido); 
            _context.SaveChanges(); 
        }

        public List<PedidoResponseDto> GetAll()
        {
            var pedidos = _context.Pedidos 
                                 .Include(p => p.Itens)
                                     .ThenInclude(item => item.Produto)
                                 .ToList(); 

       
            var pedidosDto = pedidos.Select(p => new PedidoResponseDto
            {
                Id = p.Id,
                DataCriacao = p.DataCriacao,
                DataEntrega = p.DataEntrega,
                Status = p.Status,
                TotalPedido = p.TotalPedido,
                Endereco = p.Endereco,
                NomeCliente = p.NomeCliente,
                Itens = p.Itens.Select(item => new ItemPedidoResponseDto
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    PrecoTotalItem = item.PrecoTotalItem,
                    Produto = item.Produto != null ? new ProdutoResponseDto 
                    {
                        Id = item.Produto.Id,
                        Nome = item.Produto.Nome,
                        Descricao = item.Produto.Descricao,
                        Preco = item.Produto.Preco,
                        ImagemUrl = item.Produto.ImagemUrl,
                        Categoria = item.Produto.Categoria,
                        QuantidadeMaxima = item.Produto.QuantidadeMaxima,
                        Status = item.Produto.Status
                    } : null
                }).ToList()
            }).ToList();

            return pedidosDto;
        }

        public PedidoResponseDto GetById(int id)
        {
            var pedido = _context.Pedidos 
                                 .Include(p => p.Itens)
                                     .ThenInclude(item => item.Produto)
                                 .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return null; 
            }

            var pedidoDto = new PedidoResponseDto
            {
                Id = pedido.Id,
                DataCriacao = pedido.DataCriacao,
                DataEntrega = pedido.DataEntrega,
                Status = pedido.Status,
                TotalPedido = pedido.TotalPedido,
                Endereco = pedido.Endereco,
                NomeCliente = pedido.NomeCliente,
                Itens = pedido.Itens.Select(item => new ItemPedidoResponseDto
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    PrecoTotalItem = item.PrecoTotalItem,
                    Produto = item.Produto != null ? new ProdutoResponseDto
                    {
                        Id = item.Produto.Id,
                        Nome = item.Produto.Nome,
                        Descricao = item.Produto.Descricao,
                        Preco = item.Produto.Preco,
                        ImagemUrl = item.Produto.ImagemUrl,
                        Categoria = item.Produto.Categoria,
                        QuantidadeMaxima = item.Produto.QuantidadeMaxima,
                        Status = item.Produto.Status
                    } : null
                }).ToList()
            };

            return pedidoDto;
        }

        public void Update(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public void CancelarPedido(int id)
        {
            var pedido = _context.Pedidos
                                 .Include(p => p.Itens)
                                     .ThenInclude(item => item.Produto)
                                 .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                throw new ArgumentException("Pedido não encontrado.");
            }

            if (pedido.Status == "Finalizado" || pedido.Status == "Cancelado")
            {
                throw new InvalidOperationException($"Não é possível cancelar um pedido com status '{pedido.Status}'.");
            }

            
            foreach (var item in pedido.Itens)
            {
                var produto = item.Produto; 
                if (produto != null)
                {
                    produto.QuantidadeMaxima += item.Quantidade;
                    _context.Produto.Update(produto);
                }
            }

            pedido.Status = "Cancelado";
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public void FinalizarPedido(int id)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido == null)
            {
                throw new ArgumentException("Pedido não encontrado.");
            }

            if (pedido.Status == "Finalizado" || pedido.Status == "Cancelado")
            {
                throw new InvalidOperationException($"Não é possível finalizar um pedido com status '{pedido.Status}'.");
            }

            pedido.Status = "Finalizado";
            pedido.DataEntrega = DateTime.UtcNow; 
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public decimal GetMonthlyTotalConcludedOrders(int year, int month)
        {
            return _context.Pedidos
                           .Where(p => p.Status == "Finalizado" &&
                                       p.DataEntrega.HasValue && 
                                       p.DataEntrega.Value.Year == year &&
                                       p.DataEntrega.Value.Month == month)
                           .Sum(p => p.TotalPedido);
        }
    }
}