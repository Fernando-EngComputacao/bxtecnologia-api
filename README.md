# BXTecnologia.API

API REST para gerenciamento de clientes e imagens de perfil com armazenamento seguro no AWS S3.

## 📋 Sobre o Projeto

BXTecnologia.API é uma API REST desenvolvida em .NET 8 que oferece funcionalidades para gerenciamento de clientes e suas imagens de perfil. O sistema permite o upload, atualização, visualização e exclusão de imagens de perfil, com armazenamento seguro no Amazon S3.

## 🚀 Tecnologias Utilizadas

- **.NET 8**: Framework moderno para desenvolvimento de aplicações
- **ASP.NET Core**: Framework para desenvolvimento de APIs RESTful
- **AWS S3**: Serviço de armazenamento de objetos da Amazon armazenar as imagens dos usuários
- **AWS DynamoDB**: Banco de dados NoSQL para armazenamento de dados dos clientes
- **MailKit**: Biblioteca para envio de e-mails
- **AutoMapper**: Mapeamento entre objetos
- **FluentValidation**: Validação de modelos
- **SixLabors.ImageSharp**: Processamento e manipulação de imagens
- **Swagger/OpenAPI**: Documentação da API
- **Docker**: Containerização da aplicação

## 🏗️ Arquitetura

O projeto segue uma arquitetura limpa com separação clara de responsabilidades:

- **Core**: Contém componentes centrais e reutilizáveis
- **src**: Contém a implementação específica da aplicação

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
- Limitação de uploads para usuários do plano Beginner (5 imagens por dia)

### Sistema de Validação
- Tratamento padronizado de exceções
- Validação de entradas com FluentValidation
- Respostas de erro consistentes em formato JSON

## 📦 Estrutura do Projeto

```
BXTecnologia.API/
├── Core/                     # Componentes centrais reutilizáveis
│   ├── Config/               # Configurações da aplicação
│   │   ├── Interfaces/       # Interfaces para configurações
│   │   └── AwsConfig.cs      # Configurações da AWS
│   ├── Profiles/             # Perfis de AutoMapper
│   ├── Utils/                # Utilitários e helpers
│   └── Validation/           # Sistema de validação e tratamento de exceções
│       ├── ApiException.cs   # Exceção personalizada para a API
│       └── ExceptionMiddleware.cs # Middleware para tratamento de exceções
├── src/                      # Código fonte principal da aplicação
│   ├── Controllers/          # Controladores da API
│   │   ├── CustomerController.cs      # Endpoints para clientes
│   │   └── CustomerImageController.cs # Endpoints para imagens
│   ├── Models/               # Modelos de dados
│   │   └── Customer/         # Modelos relacionados a clientes
│   │       └── DTO/          # Objetos de transferência de dados
│   ├── Repositories/         # Acesso a dados
│   │   ├── Interfaces/       # Interfaces para repositórios
│   │   └── CustomerRepository.cs # Implementação do repositório de clientes
│   └── Services/             # Serviços da aplicação
│       ├── Interfaces/       # Interfaces para serviços
│       ├── Validators/       # Validadores de FluentValidation
│       ├── CustomerService.cs    # Serviço de clientes
│       ├── EmailService.cs       # Serviço de e-mails
│       └── CustomerImageService.cs # Serviço de imagens
└── Program.cs                # Configuração da aplicação
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

## 🔐 Armazenamento de Imagens

As imagens são armazenadas de forma segura no Amazon S3, com as seguintes características:

- Bucket: `bxtecnologiabucket`
- Suporte para identificadores não-GUID (como "2-2ke")
- Processamento de imagens para otimização
- Limitação de uploads baseada no nível do usuário

## 🛡️ Sistema de Validação e Tratamento de Exceções

O projeto implementa um sistema robusto de validação e tratamento de exceções:

- **ApiException**: Exceção personalizada que inclui código de status HTTP e detalhes do erro
- **ExceptionMiddleware**: Middleware que captura exceções e retorna respostas de erro padronizadas
- **Validadores**: Implementados com FluentValidation para validar entradas

## 🚀 Como Executar

### Pré-requisitos

- .NET 8 SDK
- Conta AWS com acesso ao S3 e DynamoDB
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
   
## Desenvolvedor

Desenvolvido por Fernando Furtado © 2025

##### - Acesse o contênier no [Docker - clique aqui](https://hub.docker.com/r/furtadofernando/bxtecnologia-api)
##### - Acesse o GitHub [GitHub - clique aqui](https://github.com/Fernando-EngComputacao/bxtecnologia-api)