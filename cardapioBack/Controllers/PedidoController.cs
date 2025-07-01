using AprendendoAPI.Infraestrutura;
using AprendendoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AprendendoAPI.ViewModel;

namespace AprendendoAPI.Controllers
{
    [ApiController]
    [Route("api/pedidos")] 
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository; 

        public PedidoController(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }


        [HttpPost]
        public IActionResult CriarPedido([FromBody] PedidoViewModel pedidoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var novoPedido = new Pedido
            {
                NomeCliente = pedidoViewModel.NomeCliente,
                Endereco = pedidoViewModel.Endereco,
                DataCriacao = DateTime.UtcNow, 
                Status = "Pendente" 
            };


            novoPedido.Itens = new List<ItemPedido>();
            try
            {
                foreach (var itemViewModel in pedidoViewModel.Itens)
                {
                    var produto = _produtoRepository.BuscarPorId(itemViewModel.ProdutoId);

                    if (produto == null)
                    {
                        return BadRequest($"Produto com ID {itemViewModel.ProdutoId} não encontrado.");
                    }

                    if (produto.Status == "Indisponivel")
                    {
                        return BadRequest($"Produto '{produto.Nome}' está indisponível e não pode ser pedido.");
                    }

                    if (itemViewModel.Quantidade <= 0)
                    {
                        return BadRequest($"Quantidade inválida ({itemViewModel.Quantidade}) para o produto '{produto.Nome}'.");
                    }

                    if (itemViewModel.Quantidade > produto.QuantidadeMaxima)
                    {
                        return BadRequest($"Quantidade solicitada ({itemViewModel.Quantidade}) para '{produto.Nome}' excede a quantidade máxima permitida ({produto.QuantidadeMaxima}).");
                    }


                    novoPedido.Itens.Add(new ItemPedido
                    {
                        ProdutoId = itemViewModel.ProdutoId,
                        Quantidade = itemViewModel.Quantidade,
                    });
                }

                _pedidoRepository.Add(novoPedido); 

                return Ok(new { mensagemSucesso = "Pedido criado com sucesso!", pedidoId = novoPedido.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagemErro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagemErro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagemErro = "Ocorreu um erro interno ao processar o pedido.", detalhe = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllPedidos(
            [FromQuery] string sortBy = "status", 
            [FromQuery] string sortOrder = "asc" 
        )
        {
            var pedidos = _pedidoRepository.GetAll();

            if (sortBy.ToLower() == "status")
            {
                pedidos = pedidos.OrderBy(p => p.Status == "Pendente" ? 0 :
                                            p.Status == "Finalizado" ? 1 : 2) 
                                 .ThenBy(p => p.DataCriacao) 
                                 .ToList();
            }
            else if (sortBy.ToLower() == "datacriacao")
            {
                pedidos = sortOrder.ToLower() == "desc" ? pedidos.OrderByDescending(p => p.DataCriacao).ToList() :
                                                           pedidos.OrderBy(p => p.DataCriacao).ToList();
            }

            return Ok(pedidos);
        }


        [HttpGet("{id}")]
        public IActionResult GetPedidoPorId(int id)
        {
            var pedido = _pedidoRepository.GetById(id);

            if (pedido == null)
            {
                return NotFound("Pedido não encontrado.");
            }
            return Ok(pedido);
        }

        [HttpPut("{id}/cancel")]
        public IActionResult CancelarPedido(int id)
        {
            try
            {
                _pedidoRepository.CancelarPedido(id);
                return Ok(new { mensagemSucesso = "Pedido cancelado com sucesso e estoque atualizado." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagemErro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagemErro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagemErro = "Ocorreu um erro ao cancelar o pedido.", detalhe = ex.Message });
            }
        }

        [HttpPut("{id}/finish")]
        public IActionResult FinalizarPedido(int id)
        {
            try
            {
                _pedidoRepository.FinalizarPedido(id);
                return Ok(new { mensagemSucesso = "Pedido finalizado com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagemErro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagemErro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagemErro = "Ocorreu um erro ao finalizar o pedido.", detalhe = ex.Message });
            }
        }

        [HttpGet("monthlytotal/{year}/{month}")]
        public IActionResult GetMonthlyTotalConcludedOrders(int year, int month)
        {
            if (month < 1 || month > 12)
            {
                return BadRequest(new { mensagemErro = "Mês inválido. O mês deve estar entre 1 e 12." });
            }

            try
            {
                var total = _pedidoRepository.GetMonthlyTotalConcludedOrders(year, month);
                return Ok(new { year, month, totalReceived = total.ToString("N2", new CultureInfo("pt-BR")) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagemErro = "Ocorreu um erro ao calcular o total mensal.", detalhe = ex.Message });
            }
        }
    }
}