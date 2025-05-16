#  API de Vendas

API REST para gerenciamento de vendas, desenvolvida como parte do processo seletivo da Ambev. A aplicação foi construída com foco em manutenibilidade, testes e boas práticas de engenharia de software.

---

## Arquitetura Hexagonal

Este projeto foi feito com a **Arquitetura Hexagonal (Ports and Adapters)**:

- **Domínio** (regras de negócio puras),
- **Portas** (interfaces para comunicação),
- **Adaptadores** (banco de dados, API, mensageria, etc).

## Regras de Negócio

- Venda com pelo menos 1 item
- Máximo de **20 unidades por produto**
- Descontos por quantidade:
  - 4 a 9 unidades: **10%**
  - 10 a 20 unidades: **20%**
  - Menos de 4: **sem desconto**
- O valor total é **calculado internamente**, não aceito via API

## Docker
- docker-compose -f docker-compose-Marcus.yml up -d

### O que foi adicionado:
1. **Migrations com Entity Framework**:
   - Como criar, aplicar migrations e o que é necessário para trabalhar com migrations no projeto.
2. **RabbitMQ**:
   - caso queria subir a imagem docker pull rabbitmq:management
   - Como configurar o RabbitMQ no Docker.
   - Detalhes sobre o uso de RabbitMQ para publicar eventos de domínio.

## Como Rodar o Projeto

### Pré-requisitos

- Docker e Docker Compose instalados
- .NET 6 ou superior instalado (opcional, caso queira rodar fora do Docker)

### Subir com Docker

```bash
git clone https://github.com/mmarques84/TakingAvaliacao.git
cd seu-repo
docker-compose -f docker-compose-Marcus.yml up -d
executar no cmd
TakingAvaliacao-git\template\backend>docker-compose up --build -d
resultado 
[+] Running 4/4
 ✔ Container ambev_developer_evaluation_cache     Started                                                                                      2.9s
 ✔ Container ambev_developer_evaluation_nosql     Started                                                                                      2.3s
 ✔ Container ambev_developer_evaluation_webapi    Started                                                                                      2.9s
 ✔ Container ambev_developer_evaluation_database  Started  

##Caso queria testar sem o docker compose
docker pull rabbitmq:management
docker run -d --hostname my-rabbit --name rabbitmq \
  -p 5672:5672 -p 15672:15672 rabbitmq:management
docker pull postgres





### API PARA TESTE PODE ADICIONAR?   
1-cadastrar Branch
   https://localhost:7181/api/Branch
Request body
{
  "name": "teste ",
  "description": "teste",
  "document": "1233"
}
Response body

{
  "name": "teste 2",
  "description": "teste 2",
  "document": "1234",
  "sales": [],
  "id": "bab6ed63-eacd-466f-88f0-339a4108f449",
  "active": true,
  "createdAt": "2025-04-29T22:37:29.9753258Z"
}
*obs guardar o id para fazer a venda
------------
https://localhost:7181/api/Customers
Request body
{
  "name": "marcus 3",
  "email": "marc@teste.com.br",
  "city": "teste",
  "birthDate": "2025-04-29T22:38:48.313Z"
}
Response body
{
  "id": "938f43d1-dfe3-44d1-b789-9dbe4234e1ac",
  "name": "marcus 3",
  "email": "marc@teste.com.br",
  "city": "teste",
  "birthDate": "2022-04-29T22:38:48.313Z"
}
-------------

https://localhost:7181/api/Products
Request body
{
  "name": "string",
  "unitPrice": 0
}
Response body
  {
    "name": "sofá",
    "unitPrice": 1000,
    "saleItems": [],
    "id": "0d61da72-eb76-44a2-be5f-2815df7e7c5f",
    "active": true,
    "createdAt": "2025-04-28T01:00:04.180832Z"
  }
----------------------
https://localhost:7181/api/Sales
Request body
{
  "idCustomer": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "idBranch": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "saleItems": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 0
    }
  ],
  "saleDate": "2025-04-29T22:41:46.934Z"
}
Response body
  {
    "saleNumber": 3,
    "saleDate": "2025-04-28T13:41:54.798648Z",
    "idCustomer": "6cc99d25-565e-4838-a11b-adb2992759de",
    "customer": null,
    "saleItems": [],
    "totalAmount": 45,
    "idBranch": "0e8740d8-ae13-47e0-b373-1fb144a5e962",
    "branch": null,
    "isCancelled": false,
    "id": "017a69a5-13d1-4558-bc3e-c9b54ed181ce",
    "active": true,
    "createdAt": "2025-04-28T13:41:54.347988Z"
  }