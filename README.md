# BXTecnologia.API

API REST para gerenciamento de clientes e imagens de perfil com armazenamento seguro no AWS S3.

## 📋 Sobre o Projeto

BXTecnologia.API é uma API REST desenvolvida em .NET 8 que oferece funcionalidades para gerenciamento de clientes e suas imagens de perfil. O sistema permite o upload, atualização, visualização e exclusão de imagens de perfil, com armazenamento seguro no Amazon S3.

## 🚀 Tecnologias Utilizadas

- **.NET 8**: Framework moderno para desenvolvimento de aplicações
- **ASP.NET Core**: Framework para desenvolvimento de APIs RESTful
- **AWS S3**: Serviço de armazenamento de objetos da Amazon para imagens de perfil
- **MailKit**: Biblioteca para envio de e-mails
- **AutoMapper**: Mapeamento entre objetos
- **FluentValidation**: Validação de modelos
- **SixLabors.ImageSharp**: Processamento e manipulação de imagens
- **Swagger/OpenAPI**: Documentação da API
- **Docker**: Containerização da aplicação

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas:

- **Controllers**: Endpoints da API REST
- **Services**: Lógica de negócios
- **Models**: Modelos de dados e DTOs
- **Config**: Configurações da aplicação

## 🔧 Funcionalidades Principais

### Gerenciamento de Clientes
- Criação de novos clientes
- Consulta de clientes por ID
- Listagem de todos os clientes
- Exclusão de clientes

### Gerenciamento de Imagens
- Upload de imagens de perfil
- Atualização de imagens (redimensionamento)
- Visualização de imagens
- Listagem de todas as imagens de um cliente
- Exclusão de imagens

### Sistema de E-mails
- Envio de e-mails de confirmação de cadastro
- Envio de e-mails de confirmação de processamento de imagens
- Envio de e-mails de confirmação de cadastro de imagens
- Templates de e-mail personalizados com design moderno e responsivo

## 📦 Estrutura do Projeto

```
BXTecnologia.API/
├── Config/                  # Configurações da aplicação
│   ├── EmailLayout.cs       # Templates de e-mail
│   ├── EmailSettings.cs     # Configurações de e-mail
│   └── Interfaces/          # Interfaces para configurações
├── Controllers/             # Controladores da API
│   ├── CustomerController.cs       # Endpoints para clientes
│   └── CustomerImageController.cs  # Endpoints para imagens
├── Models/                  # Modelos de dados
│   └── Customer/            # Modelos relacionados a clientes
│       └── DTO/             # Objetos de transferência de dados
├── Services/                # Serviços da aplicação
│   ├── CustomerService.cs   # Serviço de clientes
│   ├── EmailService.cs      # Serviço de e-mails
│   ├── CustomerImageService.cs  # Serviço de imagens
│   └── Interfaces/          # Interfaces para serviços
└── Program.cs               # Configuração da aplicação
```

## 📝 Endpoints da API

### Clientes

- `POST /customers` - Criar um novo cliente
- `GET /customers/{id}` - Obter cliente por ID
- `GET /customers` - Listar todos os clientes
- `DELETE /customers/{id}` - Excluir cliente

### Imagens

- `POST /customers/{id}/image` - Fazer upload de imagem de perfil
- `PATCH /customers/{id}/{fileName}/image?Width={width}&Height={height}` - Atualizar imagem (redimensionar)
- `GET /customers/{id}/{fileName}/image` - Obter imagem específica
- `GET /customers/{id}/image` - Listar todas as imagens de um cliente
- `DELETE /customers/{id}/image` - Excluir imagem de perfil

## 📧 Sistema de E-mails

O sistema de e-mails utiliza templates HTML responsivos com design moderno e futurista. Existem três tipos de e-mails:

1. **E-mail de Confirmação de Cadastro de Usuário**
   - Enviado quando um novo cliente é cadastrado
   - Inclui informações sobre o nível do usuário
   - Design com ícones e elementos visuais modernos

2. **E-mail de Processamento de Imagens**
   - Enviado após o processamento de imagens
   - Mostra a quantidade de imagens processadas
   - Inclui botão para visualizar as imagens (opcional)

3. **E-mail de Confirmação de Cadastro de Imagem**
   - Enviado quando uma nova imagem é cadastrada
   - Mostra o nome da imagem cadastrada
   - Inclui botão para visualizar a imagem (opcional)

## 🔐 Armazenamento de Imagens

As imagens são armazenadas de forma segura no Amazon S3, com as seguintes características:

- Bucket: `bxtechbucket`
- Suporte para identificadores não-GUID (como "2-2ke")
- Processamento de imagens para otimização

## 🚀 Como Executar

### Pré-requisitos

- .NET 8 SDK
- Conta AWS com acesso ao S3
- Conta de e-mail para envio de notificações

### Configuração

1. Clone o repositório
   ```bash
   git clone https://github.com/seu-usuario/BXTecnologia.API.git
   cd BXTecnologia.API
   ```

2. Configure as variáveis de ambiente ou o arquivo `appsettings.json` com:
   - Credenciais AWS
   - Configurações de e-mail
   - Outras configurações necessárias

3. Execute a aplicação
   ```bash
   dotnet run
   ```

4. Acesse a documentação Swagger
   ```
   https://localhost:5001/swagger
   ```

### Usando Docker

1. Construa a imagem Docker
   ```bash
   docker build -t bxtecnologia-api .
   ```

2. Execute o container
   ```bash
   docker run -p 8080:80 bxtecnologia-api
   ```

## 📄 Licença

Este projeto está licenciado sob a licença [MIT](LICENSE).

## 👥 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

---

Desenvolvido por BX Tecnologia © 2025
