import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './RemoveProduto.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons'

const RemoveProduto = () => {
  const [produtoId, setProdutoId] = useState('');
  const [produtos, setProdutos] = useState([]);
  const navigate = useNavigate();

  const fetchProdutos = async () => {
    try {
      const response = await fetch('https://localhost:7027/api/produto', {
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

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`https://localhost:7027/api/produto/${produtoId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.ok) {
        alert('Produto removido com sucesso!');
        setProdutoId('');
        fetchProdutos(); 
      } else {
        const errorData = await response.json();
        alert(errorData.mensagemErro || 'Erro ao remover o produto!');
      }
    } catch (error) {
      alert('Erro ao remover produto: ' + error.message);
    }
  };

  return (
    <div className='cadastrar-produto-container'>

      <div className='div-header-admin'>
          <FontAwesomeIcon icon={faChevronLeft} className='icon-back-div' onClick={() => navigate("/admin/dashboard")} />
          <img className='img-logo-admin' src=".././public/Yume-logo.svg" alt="" />
      </div>

      <h2>Remover Produto</h2>
      <div className='lista-produtos'>
          <h3>Lista de Produtos</h3>
          <div className='tabela-wrapper'>
              <table className='tabela-produtos'>
                  <thead>
                      <tr>
                      <th>ID</th>
                      <th>Nome</th>
                      </tr>
                  </thead>
              </table>
              <div className='tabela-body-scroll'>
                  <table className='tabela-produtos'>
                      <tbody>
                          {produtos.map((p) => (
                              <tr key={p.id}>
                              <td>{p.id}</td>
                              <td>{p.nome}</td>
                              </tr>
                          ))}
                      </tbody>
                  </table>
              </div>
          </div>
      </div>

      <form onSubmit={handleSubmit} className='cadastrar-produto-form'>
          <input
          name='id'
          value={produtoId}
          onChange={(e) => setProdutoId(e.target.value)}
          placeholder='ID do Produto'
          required
          className='inputs-form-cadastro'
          />

          <div className='btns-cadastrar-produto'>
              <button className='btn-cadastrar-form-remove' type='submit'>Remover</button>
          </div>
      </form>
    </div>
  );
};

export default RemoveProduto;