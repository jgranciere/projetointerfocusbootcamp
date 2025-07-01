import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { useState, useContext } from 'react';
import './ProductDetails.css';
import BarraInferior from '../BarraInferior/barraInferior';
import { useCarrinho } from '../../context/CarrinhoContext';
import { ProdutosContext } from '../../context/ProdutosContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faCartShopping, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';


const ProductDetails = () => {
    const { produtos } = useContext(ProdutosContext);
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();
    const { adicionarAoCarrinho } = useCarrinho();

    const produtoSelecionado = state?.produto || state?.bebida;

    const [mensagem, setMensagem] = useState('');
    const [mensagemProdutoId, setMensagemProdutoId] = useState(null);

    const [erroAdicionar, setErroAdicionar] = useState('');

    if (!produtoSelecionado) {
        return <p>Produto não encontrado para o ID: {id}</p>;
    }

    const isProdutoDisponivel = produtoSelecionado.status === "Disponivel" && produtoSelecionado.quantidadeMaxima > 0;

    const verificarEAdicionarAoCarrinho = (produtoToAdd) => {
        setErroAdicionar(''); 

        if (!isProdutoDisponivel) {
            setErroAdicionar('Produto indisponível ou fora de estoque.');
            setTimeout(() => setErroAdicionar(''), 3000);
            return; 
        }
    
        adicionarAoCarrinho(produtoToAdd);
        setMensagem(<><FontAwesomeIcon icon={faCheck} className='icone-add-carrinho'/>Adicionado ao carrinho!</>);
        setTimeout(() => setMensagem(''), 3000);
    };

    const handleAdicionarCarrinhoPrincipal = () => {
        const produto = {
            id: produtoSelecionado.id,
            nome: produtoSelecionado.nome,
            preco: produtoSelecionado.preco,
            imagemUrl: produtoSelecionado.imagemUrl,
            status: produtoSelecionado.status, 
            quantidadeMaxima: produtoSelecionado.quantidadeMaxima,
        };
        verificarEAdicionarAoCarrinho(produto);
    };

    const handleAdicionarCarrinhoRecomendado = (produto) => {
        const produtoCompleto = produtos.find(p => p.id === produto.id); 
        if (!produtoCompleto) return; 

        if (produtoCompleto.status !== "Disponivel" || produtoCompleto.quantidadeMaxima <= 0) {
            setMensagemProdutoId(produto.id); 
            setErroAdicionar('Indisponível'); 
            setTimeout(() => {
                setMensagemProdutoId(null);
                setErroAdicionar('');
            }, 3000);
            return;
        }

        adicionarAoCarrinho(produto); 
        setMensagemProdutoId(produto.id);
        setTimeout(() => setMensagemProdutoId(null), 3000);
    };


    const comidas = produtos.filter(produto => produto.categoria !== 'bebida');
    const bebidas = produtos.filter(produto => produto.categoria === 'bebida');

    return (
        <div className='produto-detail-container'>
            <div className='produto-detail-header'>
                <div className='produto-detail-img'>
                    <FontAwesomeIcon icon={faArrowLeft} className='icone' onClick={() => navigate("/")} />
                    <img src={produtoSelecionado.imagemUrl} alt={`Imagem do ${produtoSelecionado.nome}`} />
                </div>

                <div className='produto-detail-info'>
                    <h1>{produtoSelecionado.nome}</h1>
                    <p>{produtoSelecionado.descricao}</p>
                    <span className='value-span-detail'><span className='span-rs'>R$</span>{parseFloat(produtoSelecionado.preco).toFixed(2)}</span>
                    
                    <p className="status-produto">
                        Status: <span className={`status-${produtoSelecionado.status?.toLowerCase()}`}>{produtoSelecionado.status}</span>
                    </p>
                    {produtoSelecionado.status === "Disponivel" && produtoSelecionado.quantidadeMaxima > 0 && (
                        <p className="quantidade-disponivel">
                            Disponível: {produtoSelecionado.quantidadeMaxima} unidades
                        </p>
                    )}

                    <button 
                        onClick={handleAdicionarCarrinhoPrincipal} 
                        className="botao-adicionar-carrinho"
                        disabled={!isProdutoDisponivel}
                    >
                        {isProdutoDisponivel ? 'Adicionar ao Carrinho' : 'Indisponível / Sem Estoque'}
                    </button>
                    {mensagem && <div className="mensagem-carrinho">{mensagem}</div>}
                    {erroAdicionar && <div className="mensagem-erro-carrinho">{erroAdicionar}</div>} 
                </div>

                <div className='produto-more-separete'>
                    <div className='produto-separate'>
                        <p>Escolha mais itens!</p>
                    </div>
                </div>

                <div className='produto-more-list-details'>
                    <div className='produto-more-list-cards'>
                        <h1 className='titulo-card-produtos'>Comidas</h1>
                        {comidas.map(produto => (
                            <div className='produto-recomended-card-pai' key={produto.id}> 
                                <div className='produto-recomended-card'>
                                    <button
                                        onClick={() => handleAdicionarCarrinhoRecomendado({
                                            id: produto.id,
                                            nome: produto.nome,
                                            preco: produto.preco,
                                            imagemUrl: produto.imagemUrl,
                                            status: produto.status,
                                            quantidadeMaxima: produto.quantidadeMaxima
                                        })}
                                        className="botao-adicionar-recomended"
                                        disabled={produto.status !== "Disponivel" || produto.quantidadeMaxima <= 0}
                                    >
                                        <FontAwesomeIcon icon={faPlus} className='icone' />
                                    </button>

                                    <div className='produto-recomended-name'>
                                        <p>{produto.nome}</p>
                                        <p><span className='span-rs'>R$</span>{parseFloat(produto.preco).toFixed(2)}</p>
                                    </div>
                                    <img src={produto.imagemUrl} alt={`Imagem do ${produto.nome}`} />
                                </div>
                                {mensagemProdutoId === produto.id && (
                                    <div className="mensagem-carrinho">
                                        {erroAdicionar === 'Indisponível' ?
                                            <span className='mensagem-erro-carrinho'>Indisponível</span> :
                                            <><FontAwesomeIcon icon={faCheck} className='icone-add-carrinho'/>Adicionado ao carrinho!</>
                                        }
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>

                    <div className='produto-more-list-cards'>
                        <h1 className='titulo-card-produtos'>Bebidas</h1>
                        {bebidas.map(produto => (
                            <div className='produto-recomended-card-pai' key={produto.id}> 
                                <div className='produto-recomended-card'>
                                    <button
                                        onClick={() => handleAdicionarCarrinhoRecomendado({
                                            id: produto.id,
                                            nome: produto.nome,
                                            preco: produto.preco,
                                            imagemUrl: produto.imagemUrl,
                                            status: produto.status,
                                            quantidadeMaxima: produto.quantidadeMaxima
                                        })}
                                        className="botao-adicionar-recomended"
                                        disabled={produto.status !== "Disponivel" || produto.quantidadeMaxima <= 0}
                                    >
                                        <FontAwesomeIcon icon={faPlus} className='icone' />
                                    </button>

                                    <div className='produto-recomended-name'>
                                        <p>{produto.nome}</p>
                                        <p><span className='span-rs'>R$</span>{parseFloat(produto.preco).toFixed(2)}</p>
                                    </div>
                                    <img src={produto.imagemUrl} alt={`Imagem do ${produto.nome}`} />
                                    { (produto.status !== "Disponivel" || produto.quantidadeMaxima <= 0) &&
                                        <p className="status-overlay">Indisponível</p>
                                    }
                                </div>
                                {mensagemProdutoId === produto.id && (
                                    <div className="mensagem-carrinho">
                                        {erroAdicionar === 'Indisponível' ?
                                            <span className='mensagem-erro-carrinho'>Indisponível</span> :
                                            <><FontAwesomeIcon icon={faCheck} className='icone-add-carrinho'/>Adicionado ao carrinho!</>
                                        }
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
            </div>
            <BarraInferior />
        </div>
    );
};

export default ProductDetails;