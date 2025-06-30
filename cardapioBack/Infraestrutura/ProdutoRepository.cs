using AprendendoAPI.Models;

namespace AprendendoAPI.Infraestrutura
{
    public class ProdutoRepository : IProdutoRepository
    {

        private readonly ConnectionContext _context = new ConnectionContext();
        public void Add(Produto produto)
        {
            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public List<Produto> Get()
        {
            return _context.Produto.ToList();
        }

        public void Update(Produto produto)
        {
            _context.Produto.Update(produto);
            _context.SaveChanges();
        }

        public void Delete(Produto produto)
        {
            _context.Produto.Remove(produto);
            _context.SaveChanges();
        }

        public Produto BuscarPorId(int id)
        {
            return _context.Produto.FirstOrDefault(p => p.Id == id);
        }

        //TABELA DE MAIS PEDIDOS

        public ProdutoMaisPedido BuscarMaisPedidoPorProdutoId(int id)
        {
            return _context.ProdutosMaisPedidos.FirstOrDefault(p => p.ProdutoId == id);
        }

        public void DeletarProdutoMaisPedido(ProdutoMaisPedido produtoMaisPedido)
        {
            _context.ProdutosMaisPedidos.Remove(produtoMaisPedido);
            _context.SaveChanges();
        }
        public void AddProdutoMaisPedido(ProdutoMaisPedido produto)
        {
            _context.ProdutosMaisPedidos.Add(produto);
            _context.SaveChanges();
        }

        public List<ProdutoMaisPedido> GetTodos()
        {
            return _context.ProdutosMaisPedidos.ToList();
        }

        
    }
}
