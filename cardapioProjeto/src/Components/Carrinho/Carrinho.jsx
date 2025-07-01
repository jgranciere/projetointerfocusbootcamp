import React, { useState } from 'react'; 
import Navbar from '../NavBar/Navbar';
import './carrinho.css';
import { useCarrinho } from '../../context/CarrinhoContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCartShopping, faPlus, faMinus } from '@fortawesome/free-solid-svg-icons';
import BarraInferior from '../BarraInferior/barraInferior';

const Carrinho = () => {
    const { carrinho, removerDoCarrinho, alterarQuantidade, total, limparCarrinho } = useCarrinho(); 
    
    const [showModal, setShowModal] = useState(false);
    const [nomeCliente, setNomeCliente] = useState('');
    const [endereco, setEndereco] = useState('');

    const handleOpenModal = () => {
        if (carrinho.length === 0) {
            alert('Seu carrinho está vazio. Adicione produtos antes de finalizar.');
            return;
        }
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setNomeCliente(''); 
        setEndereco('');
    };

    const handleConfirmarPedido = async () => {
        if (!nomeCliente || !endereco) {
            alert('Por favor, preencha seu nome e endereço para finalizar o pedido.');
            return;
        }

        const itensDoPedido = carrinho.map(produto => ({
            produtoId: produto.id,
            quantidade: produto.quantidade
        }));

        const pedidoData = {
            nomeCliente: nomeCliente,
            endereco: endereco,
            itens: itensDoPedido
        };

        try {
            const response = await fetch('https://localhost:7027/api/pedidos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(pedidoData)
            });

            if (response.ok) {
                alert('Pedido realizado com sucesso!');
                limparCarrinho(); 
                handleCloseModal(); 
            } else {
                const errorData = await response.json();
                const errorMessage = errorData.mensagemErro || 'Erro ao finalizar o pedido. Verifique os dados e tente novamente.';
                alert(errorMessage);
            }
        } catch (error) {
            console.error('Erro ao enviar pedido:', error);
            alert('Ocorreu um erro ao conectar com o servidor. Tente novamente mais tarde.');
        }
    };

    return (
        <div className="carrinho-container bg-light w-100 min-vh-100">
            <Navbar />
            <div className="container py-4 d-flex flex-column align-items-center">
                <h2 className="mb-3 fw-bold">Meu carrinho</h2>
                <p className="texto-resumo text-center mb-4">
                    Revise seus produtos antes de finalizar a compra.
                </p>

                {carrinho.length === 0 ? (
                    <>
                        <p className="text-muted">Seu carrinho está vazio.</p>
                        <img src="../public/vector.svg" alt="" className='img-carrinho-vazio' />
                    </>
                ) : (
                    carrinho.map((produto) => (
                        <div className="card shadow-sm mb-4 border-0 rounded-2" key={produto.id}>
                            <div className="row g-0">
                                <div className="col-4 col-md-3">
                                    <img
                                        src={produto.imagemUrl}
                                        className="img-fluid rounded-start h-100 object-fit-cover"
                                        alt={`Imagem de ${produto.nome}`}
                                        style={{ borderTopLeftRadius: '1rem', borderBottomLeftRadius: '1rem' }}
                                    />
                                </div>
                                <div className="col-8 col-md-9">
                                    <div className="card-body d-flex flex-column justify-content-between h-100 p-2">
                                        <div>
                                            <h5 className="card-title fw-semibold">{produto.nome}</h5>
                                            <p className="card-text text-muted mb-1 d-flex align-items-center justify-content-between">
                                                Quantidade:
                                                <button
                                                    className="btn-remove"
                                                    onClick={() => alterarQuantidade(produto.id, produto.quantidade - 1)}
                                                >
                                                    <FontAwesomeIcon icon={faMinus} className='icone-carrinho-sub' />
                                                </button>
                                                {produto.quantidade}
                                                <button
                                                    className="btn-add"
                                                    onClick={() => alterarQuantidade(produto.id, produto.quantidade + 1)}
                                                >
                                                    <FontAwesomeIcon icon={faPlus} className='icone-carrinho-add' />
                                                </button>
                                            </p>
                                            <p className="card-text fw-bold">
                                                <span>R$</span> {(produto.preco * produto.quantidade).toFixed(2)} 
                                            </p>
                                        </div>
                                        <div className="d-flex justify-content-between align-items-end mt-3">
                                            <button
                                                className="btn btn-outline-danger btn-sm"
                                                onClick={() => removerDoCarrinho(produto.id)}
                                            >
                                                Remover
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))
                )}

                {carrinho.length > 0 && (
                    <div className="d-flex justify-content-between align-items-center mt-2 border-top pt-4 flex-column compra-finalizada">
                        <h4 className="fw-bold ">Total: <span>R$</span> {total.toFixed(2)}</h4>
                        <button className='btn btn-success btn-lg mt-3' onClick={handleOpenModal}>
                            Finalizar Pedido
                        </button>
                    </div>
                )}
            </div>

            {showModal && (
                <div className="modal-backdrop">
                    <div className="modal-content-carrinho">
                        <h3>Dados para o Pedido</h3>
                        <form onSubmit={(e) => { e.preventDefault(); handleConfirmarPedido(); }}>
                            <div className="form-group">
                                <label htmlFor="nomeCliente">Seu Nome:</label>
                                <input
                                    type="text"
                                    id="nomeCliente"
                                    className="form-control"
                                    value={nomeCliente}
                                    onChange={(e) => setNomeCliente(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group mt-3">
                                <label htmlFor="endereco">Seu Endereço:</label>
                                <textarea
                                    id="endereco"
                                    className="form-control"
                                    value={endereco}
                                    onChange={(e) => setEndereco(e.target.value)}
                                    required
                                ></textarea>
                            </div>
                            <div className="d-flex justify-content-end mt-4">
                                <button type="button" className="btn btn-secondary me-2" onClick={handleCloseModal}>
                                    Voltar
                                </button>
                                <button type="submit" className="btn btn-primary">
                                    Confirmar Pedido
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
            <BarraInferior />
        </div>
    );
};

export default Carrinho;