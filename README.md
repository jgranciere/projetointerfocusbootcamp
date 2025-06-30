# 🍣 Yume Delivery

O aplicativo **Yume Delivery** foi desenvolvido para atender a demanda de um restaurante de comida japonesa que deseja oferecer um sistema de **pedidos online simples, funcional e com visual atrativo**.

A plataforma simula um sistema de delivery, onde os usuários podem visualizar os produtos disponíveis (como pratos e bebidas), adicionar itens ao carrinho e navegar pelas opções. 

Voltado tanto para o **público consumidor** quanto para a **administração do restaurante**, o sistema também conta com um **painel de administração protegido por autenticação**, onde o administrador pode cadastrar, editar e excluir produtos.

---

## 🎯 Objetivo Principal

Demonstrar a integração entre:

- **Frontend (React)**
- **Backend (.NET C#)**
- **Banco de dados (PostgreSQL)**

Com foco nas funcionalidades de **CRUD** e **controle de acesso (autenticação e rotas protegidas)**.

---

## ✅ Funcionalidades

### 👨‍🍳 Administrador
- Cadastro de novos **produtos e bebidas**
- Inclusão/remoção de produtos no **carrossel de destaques**
- **Edição** de informações dos produtos
- **Exclusão** de produtos do sistema
- Acesso a painel exclusivo via **login protegido**
- Rota protegida: **não acessível sem autenticação**

### 🧑‍🍽️ Usuário final
- Visualização de todos os produtos disponíveis
- Tela de **detalhes do produto**
- Adição de itens ao **carrinho de compras**
- Visualização dos itens no carrinho
- Botão de **"Finalizar Pedido"** (simulação, sem envio real)

---

## 🛠️ Tecnologias Utilizadas

### 🔙 Backend
- **.NET C#**
- **Entity Framework Core (EF Core)** com Migrations
- **PostgreSQL** como banco de dados

**Bibliotecas:**
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql` (provedor PostgreSQL)

---

### 🔜 Frontend
- **React**
- **React Router DOM** – para navegação entre páginas
- **Swiper.js** – carrossel de produtos em destaque
- **FontAwesome Icons** – ícones visuais

---

## 🛠️ Por que essas tecnologias?

Escolhi essas ferramentas por já ter tido contato com elas nas aulas da faculdade e por conta da familiaridade, o que me proporcionou maior produtividade durante o desenvolvimento.

---

## 🚀 Como rodar o projeto localmente

### Pré-requisitos:
- [.NET SDK 8+](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/)

---

### 1. Clonando o repositório:
- git clone https://github.com/jgranciere/bootcampInterfocus.git
- cd bootcampInterfocus

### 2. Executando o backend:
- cd cardapioBack
- dotnet restore
- dotnet ef database update
- dotnet run

### 3. Executando o frontend:
- cd ../cardapioProjeto
- npm install
- npm run dev

###🔐 Login do Administrador
- Usuário: admin
- Senha: admin
