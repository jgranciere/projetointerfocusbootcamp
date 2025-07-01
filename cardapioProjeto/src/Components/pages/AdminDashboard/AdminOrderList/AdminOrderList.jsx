import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronLeft, faCheckCircle, faTimesCircle, faSpinner } from '@fortawesome/free-solid-svg-icons';
import './AdminOrderList.css'; 

const AdminOrderList = () => {
    const [pedidos, setPedidos] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [sortBy, setSortBy] = useState('status'); 
    const [sortOrder, setSortOrder] = useState('asc'); 
    const [totalMensal, setTotalMensal] = useState(0);
    const navigate = useNavigate();

    const fetchPedidos = async () => {
        setLoading(true);
        setError(null);
        try {
            const pedidosResponse = await fetch(`https://localhost:7027/api/pedidos?sortBy=${sortBy}&sortOrder=${sortOrder}`);
            if (!pedidosResponse.ok) {
                throw new Error('Falha ao buscar pedidos.');
            }
            const pedidosData = await pedidosResponse.json();
            setPedidos(pedidosData);

            const now = new Date();
            const year = now.getFullYear();
            const month = now.getMonth() + 1; 
            const totalResponse = await fetch(`https://localhost:7027/api/pedidos/monthlytotal/${year}/${month}`);
            if (!totalResponse.ok) {
                throw new Error('Falha ao buscar total mensal.');
            }
            const totalData = await totalResponse.json();
            setTotalMensal(totalData.totalReceived);

        } catch (err) {
            console.error("Erro ao buscar dados:", err);
            setError('Não foi possível carregar os dados dos pedidos. Tente novamente mais tarde.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPedidos();
    }, [sortBy, sortOrder]); 

    const formatarData = (dataString) => {
        if (!dataString) return 'N/A';
        const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' };
        return new Date(dataString).toLocaleString('pt-BR', options);
    };

    const handleAction = async (pedidoId, actionType) => {
        if (!window.confirm(`Tem certeza que deseja ${actionType === 'cancel' ? 'cancelar' : 'finalizar'} este pedido?`)) {
            return;
        }

        try {
            const response = await fetch(`https://localhost:7027/api/pedidos/${pedidoId}/${actionType}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' }
            });

            if (response.ok) {
                alert(`Pedido ${actionType === 'cancel' ? 'cancelado' : 'finalizado'} com sucesso!`);
                fetchPedidos(); // Rebusca os pedidos para atualizar a lista
            } else {
                const errorData = await response.json();
                alert(errorData.mensagemErro || `Erro ao ${actionType === 'cancel' ? 'cancelar' : 'finalizar'} pedido.`);
            }
        } catch (error) {
            console.error(`Erro na ação de ${actionType}:`, error);
            alert('Erro ao conectar com o servidor.');
        }
    };

    return (
        <div className="admin-order-list-container">
            <div className='div-header-admin'>
                <FontAwesomeIcon icon={faChevronLeft} className='icon-back-div' onClick={() => navigate("/admin/dashboard")} />
                <img className='img-logo-admin' src=".././public/Yume-logo.svg" alt="Yume Logo" />
            </div>

            <h2 className="text-center my-4">Gerenciamento de Pedidos</h2>

            {error && <div className="alert alert-danger text-center">{error}</div>}

            <div className="order-summary-box text-center mb-4 p-3 rounded shadow-sm">
                <h4>Total Recebido no Mês ({new Date().toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' })}):</h4>
                <p className="total-value fw-bold">R$ {parseFloat(totalMensal).toFixed(2)}</p>
            </div>

            <div className="filters-and-sorts d-flex justify-content-center mb-4">
                <div className="me-3">
                    <label htmlFor="sortBy" className="form-label">Ordenar por:</label>
                    <select id="sortBy" className="form-select" value={sortBy} onChange={(e) => setSortBy(e.target.value)}>
                        <option value="status">Status</option>
                        <option value="dataCriacao">Data de Criação</option>
                    </select>
                </div>
                <div>
                    <label htmlFor="sortOrder" className="form-label">Ordem:</label>
                    <select id="sortOrder" className="form-select" value={sortOrder} onChange={(e) => setSortOrder(e.target.value)}>
                        <option value="asc">Ascendente</option>
                        <option value="desc">Descendente</option>
                    </select>
                </div>
            </div>

            {loading ? (
                <div className="text-center py-5">
                    <FontAwesomeIcon icon={faSpinner} spin size="2x" />
                    <p className="mt-2">Carregando pedidos...</p>
                </div>
            ) : pedidos.length === 0 ? (
                <p className="text-center text-muted">Nenhum pedido encontrado.</p>
            ) : (
                <div className="pedidos-list">
                    {pedidos.map(pedido => (
                        <div className="card mb-3 shadow-sm border-0 rounded-3" key={pedido.id}>
                            <div className="card-body">
                                <h5 className="card-title fw-bold">Pedido #{pedido.id} - Cliente: {pedido.nomeCliente}</h5>
                                <p className="card-text mb-1">Endereço: {pedido.endereco}</p>
                                <p className="card-text mb-1">Data Criação: {formatarData(pedido.dataCriacao)}</p>
                                <p className="card-text mb-1">Data Entrega: {formatarData(pedido.dataEntrega)}</p>
                                <p className="card-text fw-bold">Status: <span className={`status-${pedido.status.toLowerCase()}`}>{pedido.status}</span></p>
                                <p className="card-text fw-bold">Total: R$ {parseFloat(pedido.totalPedido).toFixed(2)}</p>

                                <h6 className="mt-3 mb-2">Itens:</h6>
                                <ul className="list-group list-group-flush">
                                    {pedido.itens && pedido.itens.map(item => (
                                        <li className="list-group-item d-flex justify-content-between align-items-center py-1" key={item.id}>
                                            {item.produto?.nome || 'Produto Desconhecido'} x {item.quantidade} - R$ {parseFloat(item.precoTotalItem).toFixed(2)}
                                        </li>
                                    ))}
                                </ul>

                                <div className="d-flex justify-content-end mt-3">
                                    {pedido.status.toLowerCase() !== 'cancelado' && pedido.status.toLowerCase() !== 'finalizado' && (
                                        <>
                                            <button 
                                                className="btn btn-danger btn-sm me-2" 
                                                onClick={() => handleAction(pedido.id, 'cancel')}
                                            >
                                                <FontAwesomeIcon icon={faTimesCircle} className="me-1" /> Cancelar
                                            </button>
                                            <button 
                                                className="btn btn-success btn-sm" 
                                                onClick={() => handleAction(pedido.id, 'finish')}
                                            >
                                                <FontAwesomeIcon icon={faCheckCircle} className="me-1" /> Finalizar
                                            </button>
                                        </>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default AdminOrderList;