
# HemoVida - Sistema de Gerenciamento de Doação de Sangue

![image](https://github.com/user-attachments/assets/e36c5d95-e574-4e7a-940a-c9f59fb93f4c)

## 🩸 Descrição do Projeto

**HemoVida** é uma aplicação desenvolvida em .NET para o gerenciamento de um banco de dados de doação de sangue. O sistema oferece uma solução completa para o cadastro de doadores, controle de estoque de sangue, notificações, geração de relatórios e integração com serviços externos, como a consulta de endereço via CEP.

A aplicação segue os princípios da arquitetura Onion e conta com um Worker para notificações assíncronas, garantindo modularidade, escalabilidade e manutenção facilitada.

## 🎯 Objetivo

Oferecer uma plataforma completa para clínicas e hemocentros gerenciarem com eficiência o processo de doação de sangue, desde o cadastro do doador até a geração de relatórios operacionais.

## 🧱 Estrutura do Projeto

A solução foi construída utilizando **Onion Architecture**, composta por:

- **Domain**: Entidades, interfaces e validações que representam o núcleo do sistema.
- **Application**: Lógica de negócio e orquestração entre camadas.
- **Infrastructure**: Acesso a dados com SQL Server e integração com APIs externas (ex: CEP).
- **API**: Exposição de endpoints RESTful com autenticação JWT e documentação Swagger.
- **Worker/Notifier**: Serviço background para envio de notificações (ex: e-mails).
- **Tests**: Testes automatizados das camadas e serviços principais.

## 🧪 Tecnologias Utilizadas

- **.NET 8**
- **C#**
- **SQL Server**
- **Kafka**
- **JWT (JSON Web Token)**
- **Docker Compose**
- **Redis**
- **SonarQube**
- **Swagger**
- **Onion Architecture**

## ⚙️ Instruções para Execução

1. **Clone o repositório**
   ```bash
   git clone https://github.com/gustavorenedev/HemoVida
   cd HemoVida
   ```

2. **Execute o Docker Compose**
   ```bash
   docker-compose up --build
   ```

3. **Acesse os serviços**
   - **HemoVida API**: `https://localhost:8081/swagger`
   - **Kafka UI**: `http://localhost:9007`

4. **Executar o Worker separadamente** no modo debug para acompanhar o fluxo de notificações.

## 📡 Endpoints da API

### 🔐 AuthController

| Método | Rota              | Descrição                                 |
|--------|-------------------|-------------------------------------------|
| POST   | `/api/Auth/Register` | Registro de novo usuário.                |
| POST   | `/api/Auth/Login`    | Autenticação e retorno de token JWT.     |

### 💉 DonationController

| Método | Rota                       | Descrição                              |
|--------|----------------------------|----------------------------------------|
| POST   | `/api/Donation/DonationRegister` | Registrar uma nova doação. (Admin)   |

### 🧍 DonorController

| Método | Rota                            | Descrição                                                  |
|--------|---------------------------------|------------------------------------------------------------|
| GET    | `/api/Donor/GetAvailableDonors` | Lista doadores que ainda não realizaram doação. (Admin)    |
| POST   | `/api/Donor/RegisterDonor`      | Registro de informações do doador (Pré-doação). (User)     |
| GET    | `/api/Donor/GetDonationHistory` | Histórico de doações por email.                            |
| GET    | `/api/Donor/GetAllDonationHistory` | Histórico geral de doações. (Admin)                    |

### 📊 ReportController

| Método | Rota                                | Descrição                                                                 |
|--------|-------------------------------------|---------------------------------------------------------------------------|
| GET    | `/api/Report/GetBloodStockReport`   | Relatório com a quantidade de sangue por tipo sanguíneo. (Admin)         |
| GET    | `/api/Report/GetDonorsLast30DaysReport` | Relatório de doações nos últimos 30 dias. (Admin)                    |

## 🧠 Regras de Negócio

- ✅ Cadastro único por e-mail
- ❌ Menores de idade não podem doar (mas podem se cadastrar)
- ⚖️ Peso mínimo de 50kg para doação
- 🚺 Mulheres: intervalo mínimo de 90 dias entre doações
- 🚹 Homens: intervalo mínimo de 60 dias entre doações
- 💉 Quantidade de sangue doado: entre 420ml e 470ml

## 📄 XML Comentado

A documentação dos endpoints foi feita com base em comentários XML, permitindo integração direta com Swagger para uma experiência completa de documentação e testes automatizados via interface web.

## ✅ Conclusão

O **HemoVida** é um sistema completo, moderno e escalável, que reúne boas práticas de arquitetura de software, segurança e eficiência operacional. Ideal para ambientes que demandam controle rigoroso de doações de sangue, promove confiabilidade e automação no processo de coleta, validação e notificação.
