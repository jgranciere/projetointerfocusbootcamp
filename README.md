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

- ⚠️ Observação:
Caso o comando dotnet ef database update não funcione corretamente ou não crie a tabela automaticamente, acesse seu banco de dados PostgreSQL (por exemplo, pelo pgAdmin) e crie manualmente uma tabela chamada "teste_produtos". Depois, rode novamente o projeto.

### 3. Executando o frontend:
- cd ../cardapioProjeto
- npm install
- npm run dev

###🔐 Login do Administrador
- Usuário: admin
- Senha: admin
