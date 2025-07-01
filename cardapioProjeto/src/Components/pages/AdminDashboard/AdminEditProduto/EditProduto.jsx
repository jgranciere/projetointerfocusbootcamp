import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './EditProduto.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons'

const EditProduto = () => {
    const [produtos, setProdutos] = useState([]);
    const [produtoSelecionado, setProdutoSelecionado] = useState({
        id: '',
        nome: '',
        descricao: '',
        preco: '',
        quantidadeMaxima: '',
        status: ''
    });
    const navigate = useNavigate();
    const [mostrarPainel, setMostrarPainel] = useState(false);

    const fetchProdutos = async () => {
        try {
            const response = await fetch('https://localhost:7027/api/produto?pageSize=999', { 
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                const data = await response.json();
                setProdutos(data.products || []); 
            } else {
                alert('Erro ao carregar produtos.');
            }
        } catch (error) {
            alert('Erro ao buscar produtos: ' + error.message);
        }
    };

    useEffect(() => {
        fetchProdutos();
    }, []);

    const handleProdutoClick = (produto) => {
        setProdutoSelecionado(produto); 
        setMostrarPainel(true);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setProdutoSelecionado(prev => ({
            ...prev,
            [name]: value 
        }));
    };

    const handleFocus = () => {
        setMostrarPainel(true);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const precoNumerico = parseFloat(String(produtoSelecionado.preco).replace(',', '.'));
        if (isNaN(precoNumerico)) {
            alert('Preço inválido. Use números e ponto como separador decimal.');
            return;
        }

        try {
            const response = await fetch(`https://localhost:7027/api/produto/${produtoSelecionado.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    nome: produtoSelecionado.nome,
                    preco: precoNumerico, 
                    descricao: produtoSelecionado.descricao,
                    quantidadeMaxima: parseInt(produtoSelecionado.quantidadeMaxima), 
                    status: produtoSelecionado.status
                }),
            });

            if (response.ok) {
                alert('Produto editado com sucesso!');
                setMostrarPainel(false); 
                fetchProdutos(); 
            } else {
                const errorData = await response.json(); 
                alert(errorData.mensagemErro || 'Erro ao editar produto. Verifique os dados e tente novamente.');
            }
        } catch (error) {
            alert('Erro ao editar produto: ' + error.message);
        }
    };

    return (
        <div className='cadastrar-produto-container container-edit'>
            <div className='div-header-admin'>
                <FontAwesomeIcon icon={faChevronLeft} className='icon-back-div' onClick={() => navigate("/admin/dashboard")} />
                <img className='img-logo-admin' src=".././public/Yume-logo.svg" alt="" />
            </div>

            <h2>Editar Produto</h2>
            <div className='lista-produtos'>

                <h3>Lista de Produtos</h3>
                <div className='tabela-wrapper'>
                    <table className='tabela-produtos'>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Nome</th>
                                <th>Qtd. Máxima</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                    </table>
                    <div className='tabela-body-scroll'>
                        <table className='tabela-produtos'>
                            <tbody>
                                {produtos.map((p) => (
                                    <tr key={p.id} onClick={() => handleProdutoClick(p)} style={{ cursor: 'pointer' }}>
                                        <td onFocus={handleFocus}>{p.id}</td>
                                        <td>{p.nome}</td>
                                        <td>{p.quantidadeMaxima}</td>
                                        <td>{p.status}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            {mostrarPainel && (
                <div className="overlay-edicao">
                    <div className="painel-edicao">
                        <form onSubmit={handleSubmit} className='cadastrar-produto-form'>
                            <input
                                name='nome'
                                value={produtoSelecionado.nome}
                                onChange={handleChange}
                                placeholder='Nome'
                                className='inputs-form-cadastro'
                                required
                            />
                            <textarea
                                name='descricao'
                                value={produtoSelecionado.descricao}
                                onChange={handleChange}
                                placeholder='Descrição'
                                className='inputs-form-cadastro text-area-descricao'
                                required
                            />
                            <input
                                name='preco'
                                type='number'
                                step='0.01' 
                                value={produtoSelecionado.preco}
                                onChange={handleChange}
                                placeholder='Preço'
                                className='inputs-form-cadastro '
                                required
                            />
                            <input
                                name='quantidadeMaxima'
                                type='number'
                                value={produtoSelecionado.quantidadeMaxima}
                                onChange={handleChange}
                                placeholder='Quantidade Máxima'
                                className='inputs-form-cadastro'
                                required
                            />
                            <select
                                name='status'
                                value={produtoSelecionado.status}
                                onChange={handleChange}
                                className='inputs-form-cadastro'
                                required
                            >
                                <option value="">Selecione o Status</option>
                                <option value="Disponivel">Disponível</option>
                                <option value="Indisponivel">Indisponível</option>
                            </select>

                            <div className='btns-cadastrar-produto'>
                                <button className='btn-cadastrar-form' type='submit'>Salvar Alterações</button>
                            </div>
                        </form>
                        <button type="button" className='btn-cadastrar-form-remove-painel' onClick={() => setMostrarPainel(false)}>Sair</button>

                    </div>
                </div>
            )}
        </div>
    );
};

export default EditProduto;