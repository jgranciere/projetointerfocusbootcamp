import React, { useEffect, useState } from 'react'; 
import './ListaProdutos.css';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faChevronLeft, faChevronRight } from '@fortawesome/free-solid-svg-icons';

const Listaprodutos = ({ termoBusca }) => {
    const [produtosExibidos, setProdutosExibidos] = useState([]); 
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [sortBy, setSortBy] = useState('nome'); 
    const [sortOrder, setSortOrder] = useState('asc'); 
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(12); 
    const [totalPages, setTotalPages] = useState(1); 

    const fetchProdutos = async () => {
        setLoading(true);
        setError(null);
        try {
            const url = `https://localhost:7027/api/produto?sortBy=${sortBy}&sortOrder=${sortOrder}&page=${currentPage}&pageSize=${pageSize}&searchTerm=${termoBusca}`;
            
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error('Falha ao carregar produtos.');
            }
            const data = await response.json();
            
            setProdutosExibidos(data.products || []); 
            setTotalPages(Math.ceil(data.totalCount / pageSize));

        } catch (err) {
            console.error('Erro ao buscar produtos:', err);
            setError('Não foi possível carregar os produtos. Tente novamente mais tarde.');
        } finally {
            setLoading(false);
        }
    };


    useEffect(() => {
        fetchProdutos();
    }, [sortBy, sortOrder, currentPage, termoBusca]); 

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            setCurrentPage(prevPage => prevPage + 1);
        }
    };

    const handlePrevPage = () => {
        if (currentPage > 1) {
            setCurrentPage(prevPage => prevPage - 1);
        }
    };

    const comidasExibidas = produtosExibidos.filter(produto => produto.categoria !== 'bebida');
    const bebidasExibidas = produtosExibidos.filter(produto => produto.categoria === 'bebida');

    return (
        <div className='container-lista-produtos'>
            <div className='container-produtos'>
                
                {error && <div className="alert alert-danger text-center">{error}</div>}
                {loading ? (
                    <p className="text-center">Carregando produtos...</p>
                ) : (
                    <>
                    
                        <div className="filters-sorts-pagination d-flex justify-content-center align-items-center mb-4">
                            <div className="me-3">
                                <label htmlFor="sortBy" className="form-label">Ordenar por:</label>
                                <select id="sortBy" className="form-select" value={sortBy} onChange={(e) => {
                                    setSortBy(e.target.value);
                                    setCurrentPage(1);
                                }}>
                                    <option value="nome">Nome</option>
                                    <option value="preco">Preço</option>
                                </select>
                                
                            </div>
                            <div>
                                <label htmlFor="sortOrder" className="form-label">Ordem:</label>
                                <select id="sortOrder" className="form-select" value={sortOrder} onChange={(e) => {
                                    setSortOrder(e.target.value);
                                    setCurrentPage(1);
                                }}>
                                    <option value="asc">Ascendente</option>
                                    <option value="desc">Descendente</option>
                                </select>
                            </div>
                        </div>
                        <h1>Produtos</h1>

                        <div className='container-produtos-list'>
                            <div className='produto-card'>
                                {comidasExibidas.length > 0 ? (
                                    comidasExibidas.map(produto => (
                                        <Link to={`/produto/${produto.id}`} state={{ produto }} key={produto.id} className='cards'>
                                            <div className='infos-produtos'>
                                                <div className='button-add-product'>
                                                    <button className='add-product'><FontAwesomeIcon icon={faPlus} className='icone-carrinho-add' /></button>
                                                </div>
                                                <div className='infos-img-card-product'>
                                                    <div className='infos-card-product'>
                                                        <h1>{produto.nome}</h1>
                                                        <span><span className='span-rs'>R$</span>{parseFloat(produto.preco).toFixed(2)}</span>
                                                    </div>
                                                    <div className='div-infos-img'>
                                                        <div className='infos-imagem'>
                                                            <img src={produto.imagemUrl} alt={`Imagem do ${produto.nome}`} />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </Link>
                                    ))
                                ) : (
                                    <p className="text-center col-12">Nenhuma comida encontrada.</p>
                                )}
                            </div>

                            <h1>Bebidas</h1>
                            <div className='produto-card'>
                                {bebidasExibidas.length > 0 ? (
                                    bebidasExibidas.map(produto => (
                                        <Link to={`/produto/${produto.id}`} state={{ produto }} key={produto.id} className='cards'>
                                            <div className='infos-produtos'>
                                                <div className='button-add-product'>
                                                    {/* Lembre-se de adicionar lógica de adicionar ao carrinho aqui */}
                                                    <button className='add-product'><FontAwesomeIcon icon={faPlus} className='icone-carrinho-add' /></button>
                                                </div>
                                                <div className='infos-img-card-product'>
                                                    <div className='infos-card-product'>
                                                        <h1>{produto.nome}</h1>
                                                        <span><span className='span-rs'>R$</span>{parseFloat(produto.preco).toFixed(2)}</span>
                                                    </div>
                                                    <div className='div-infos-img'>
                                                        <div className='infos-imagem'>
                                                            <img src={produto.imagemUrl} alt={`Imagem do ${produto.nome}`} />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </Link>
                                    ))
                                ) : (
                                    <p className="text-center col-12">Nenhuma bebida encontrada.</p>
                                )}
                            </div>
                        </div>

                        {totalPages > 1 && (
                            <div className="pagination-controls d-flex justify-content-center mt-4">
                                <div>
                                    <span className="align-self-center mx-2">Página {currentPage} de {totalPages}</span>
                                </div>
                                
                                <div className='btns-pagination'>
                                    <button
                                        className="btn btn-outline-secondary me-2"
                                        onClick={handlePrevPage}
                                        disabled={currentPage === 1}
                                    >
                                        <FontAwesomeIcon icon={faChevronLeft} /> 
                                    </button>
                                    
                                    <button
                                        className="btn btn-outline-secondary ms-2"
                                        onClick={handleNextPage}
                                        disabled={currentPage === totalPages}
                                    >
                                         <FontAwesomeIcon icon={faChevronRight} />
                                    </button>
                                </div>

                                
                            </div>
                        )}
                    </>
                )}
            </div>
        </div>
    );
};

export default Listaprodutos;