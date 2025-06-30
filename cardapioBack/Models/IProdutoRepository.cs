namespace AprendendoAPI.Models
{
    public interface IProdutoRepository
    {
        void Add(Produto produto);

        List<Produto> Get();

        void Update(Produto produto);

        void Delete(Produto produto);

        Produto BuscarPorId(int id);

        void AddProdutoMaisPedido(ProdutoMaisPedido produto);

        List<ProdutoMaisPedido> GetTodos();

        void DeletarProdutoMaisPedido(ProdutoMaisPedido produtoMaisPedido);

        ProdutoMaisPedido BuscarMaisPedidoPorProdutoId(int id);




    }
}
