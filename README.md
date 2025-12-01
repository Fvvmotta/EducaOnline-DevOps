# **EducaOnline – Plataforma Modular de Gestão de Cursos e Alunos**

## **1. Visão Geral**

O **EducaOnline** é uma plataforma corporativa distribuída, construída com **arquitetura de microsserviços**, para **gestão completa de cursos, alunos, matrículas, pagamentos e certificados**, integrando múltiplos domínios via **mensageria RabbitMQ** e um **BFF (Backend for Frontend)** em .NET e um **frontend em Angular**.

O projeto foi desenvolvido como parte do MBA **DevXpert Full Stack .NET**, no módulo **Construção de Aplicações Corporativas**, aplicando **DDD (Domain-Driven Design)**, **CQRS**, **Event-Driven Architecture** e **Clean Architecture**.

---

## **2. Autores**

- **Jairo Bionez**
- **Fernando Vinícius Valim Motta**
- **Victor Lino**
- **Ozias Manoel Costa Neto**

---

## **3. Arquitetura do Projeto**

A solução é organizada em **camadas independentes**:

```
EducaOnline/
│
├── backend/
│   └── src/
│       ├── ApiGateways/
│       │   └── EducaOnline.Bff/                  → BFF central que integra os domínios e o frontend
│       ├── BuildingBlocks/
│       │   ├── EducaOnline.Core/                 → Domínios compartilhados, validações, eventos
│       │   ├── EducaOnline.MessageBus/           → Implementação do barramento RabbitMQ
│       │   └── EducaOnline.WebAPI.Core/          → Middlewares, JWT e extensões de API
│       ├── Services/
│       │   ├── Aluno/
│       │   │   └── EducaOnline.Aluno.API/        → Contexto de alunos, matrículas e certificados
│       │   ├── Conteudo/
│       │   │   └── EducaOnline.Conteudo.API/     → Contexto de cursos e aulas
│       │   ├── Financeiro/
│       │   │   ├── EducaOnline.Financeiro.API/   → Contexto de faturamento e pagamentos
│       │   │   └── EducaOnline.Financeiro.Pagamentos/
│       │   ├── Identidade/
│       │   │   └── EducaOnline.Identidade.API/   → Autenticação e autorização (JWT)
│       │   └── Pedidos/
│       │       └── EducaOnline.Pedidos.API/      → Contexto de pedidos e integração financeira
│
└── frontend/
    ├── apps/                                     → Aplicações Angular (Portal do Aluno, Admin, etc.)
    ├── libs/                                     → Módulos e componentes compartilhados
    ├── package.json                              → Configuração de dependências
    └── README.md                                 → Documentação específica do frontend
```

---

## **4. Pré-requisitos**

Antes de executar o projeto, certifique-se de ter instalado:

### **Obrigatórios:**

#### **.NET SDK 9.0** ou superiora

- Download: https://dotnet.microsoft.com/download
- Verificação: `dotnet --version`

#### **Node.js 20+** (LTS) e npm

- Download: https://nodejs.org/
- Verificação:
  ```bash
  node --version
  npm --version
  ```

#### **Docker Desktop**

- Download: https://www.docker.com/products/docker-desktop
- Verificação: `docker --version`
- **Importante**: O Docker deve estar **rodando** antes de iniciar o projeto!

### **Opcionais (Recomendados):**

- **Visual Studio 2022** (Community, Professional ou Enterprise)
- **Visual Studio Code** com extensões:
  - C# Dev Kit
  - Docker
  - Angular Language Service
- **Git** para controle de versão

---

## **5. Configuração Inicial - Passo a Passo**

### **Passo 1: Clonar o Repositório**

```bash
git clone https://github.com/Hellstricker/EducaOnline.git
cd EducaOnline
```

---

### **Passo 2: Iniciar o RabbitMQ (OBRIGATÓRIO)**

**CRÍTICO**: O RabbitMQ deve estar rodando **ANTES** de qualquer API!

```bash
docker run -d --hostname educa-rabbit --name educa-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

**Verificar se está rodando:**

```bash
docker ps | grep educa-rabbit
```

Você deve ver algo como:

```
CONTAINER ID   IMAGE                    STATUS          PORTS
abc123def456   rabbitmq:3-management    Up 30 seconds   0.0.0.0:5672->5672/tcp, 0.0.0.0:15672->15672/tcp
```

**Acessar painel de gerenciamento:**

- URL: http://localhost:15672
- Usuário: `guest`
- Senha: `guest`

**IMPORTANTE**: Aguarde **30-60 segundos** após iniciar o RabbitMQ antes de executar as APIs!

**Se o container já existir mas estiver parado:**

```bash
docker start educa-rabbit
```

---

### **Passo 3: Instalar Dependências do Frontend**

```bash
cd frontend
npm install
```

Este processo pode demorar 2-5 minutos na primeira execução.

---

## **6. Executando a Solução**

### **IMPORTANTE: Inicialização Automática**

**O projeto está configurado para criar automaticamente:**

- Bancos de dados SQLite
- Todas as tabelas necessárias
- Dados iniciais (seed) em ambiente Development

**NÃO é necessário rodar migrations manualmente!**

Tudo acontece automaticamente ao executar `dotnet run` em cada serviço.

---

### **Opção 1: Executar via Terminal (Recomendado para Depuração)**

**Execute nesta ordem obrigatória:**

#### **1. Identidade.API** (deve ser o primeiro)

```bash
cd backend/src/Services/EducaOnline.Identidade.API
dotnet run
```

Aguarde a mensagem: `Now listening on: https://localhost:7070`

Logs esperados:

```
Verificando banco de dados...
Banco de dados verificado/criado com sucesso!
Now listening on: https://localhost:7070
```

---

#### **2. Conteudo.API**

```bash
# Em um NOVO terminal
cd backend/src/Services/EducaOnline.Conteudo.API
dotnet run
```

Aguarde: `Now listening on: https://localhost:7183`

---

#### **3. Aluno.API**

```bash
# Em um NOVO terminal
cd backend/src/Services/EducaOnline.Aluno.API
dotnet run
```

Se falhar com erro de RabbitMQ, verifique se o Docker está rodando!

---

#### **4. Pedidos.API**

```bash
# Em um NOVO terminal
cd backend/src/Services/EducaOnline.Pedidos.API
dotnet run
```

---

#### **5. Financeiro.API**

```bash
# Em um NOVO terminal
cd backend/src/Services/EducaOnline.Financeiro.API
dotnet run
```

---

#### **6. BFF (Gateway)**

```bash
# Em um NOVO terminal
cd backend/src/api_gateways/EducaOnline.Bff
dotnet run
```

Aguarde: `Now listening on: https://localhost:7093`

---

#### **7. Frontend Angular**

```bash
# Em um NOVO terminal
cd frontend
npx nx serve educa-online
```

Acesse: http://localhost:4200

---

### **Opção 2: Executar via Visual Studio 2022**

1. Abra a solution `EducaOnline.sln`
2. Clique com botão direito na Solution → **Configure Startup Projects**
3. Selecione **Multiple startup projects**
4. Configure **nesta ordem** (importante!):
   - EducaOnline.Identidade.API → **Start**
   - EducaOnline.Conteudo.API → **Start**
   - EducaOnline.Aluno.API → **Start**
   - EducaOnline.Pedidos.API → **Start**
   - EducaOnline.Financeiro.API → **Start**
   - EducaOnline.Bff → **Start**
5. Pressione **F5** ou clique em **Start**

O frontend Angular precisa ser iniciado separadamente no terminal.

---

## **7. Serviços e Portas**

| Projeto                              | Porta HTTPS | Porta HTTP | Swagger                        |
| ------------------------------------ | ----------- | ---------- | ------------------------------ |
| **Frontend Angular**           | -           | 4200       | http://localhost:4200          |
| **EducaOnline.Bff**            | 7093        | 5051       | https://localhost:7093/swagger |
| **EducaOnline.Identidade.API** | 7070        | 5244       | https://localhost:7070/swagger |
| **EducaOnline.Conteudo.API**   | 7183        | 5105       | https://localhost:7183/swagger |
| **EducaOnline.Aluno.API**      | 7094        | 5152       | https://localhost:7094/swagger |
| **EducaOnline.Pedidos.API**    | 7244        |            | https://localhost:7244/swagger |
| **EducaOnline.Financeiro.API** | 7059        |            | https://localhost:7059/swagger |
| **RabbitMQ Management**        | -           | 15672      | http://localhost:15672         |

**Nota**: Algumas portas podem variar conforme o `launchSettings.json` de cada projeto.

---

## **8. Dados Iniciais (Seed)**

Em ambiente **Development**, cada serviço cria dados automaticamente na primeira execução:

### **Identidade.API**

Cria 2 usuários padrão:

**Administrador:**

- Email: `admin@educaonline.com.br`
- Senha: `Teste@123`
- Perfil: Administrador

**Aluno:**

- Email: `aluno@educaonline.com.br`
- Senha: `Teste@123`
- ID fixo: `40640fec-5daf-4956-b1c0-2fde87717b66`
- Perfil: Aluno

---

### **Conteudo.API**

Cria 3 cursos:

1. **Introdução à Inteligência Artificial**

   - ID: `04effc8b-fa4a-415c-90eb-95cdfdaba1b2`
   - Carga horária: 20h
   - Total de aulas: 2
2. **Desenvolvimento Web com Angular**

   - ID: `04effc8b-fa4a-415c-90eb-95cdfdaba1b7`
   - Carga horária: 20h
   - Total de aulas: 2
3. **Arquitetura de Software com .NET**

   - ID: `04effc8b-fa4a-415c-90eb-95cdfdaba1b8`
   - Carga horária: 20h
   - Total de aulas: 2

---

### **Aluno.API**

- Cria o aluno com mesmo ID do Identity
- Cria matrícula automática nos 3 cursos
- Registra progresso em 1 aula (50%)
- Emite certificado do Curso IA

---

## **9. Testando a Aplicação**

### **9.1. Autenticação via Swagger**

1. Acesse: https://localhost:7070/swagger
2. Localize o endpoint `POST /api/identidade/autenticar`
3. Clique em **"Try it out"**
4. Use as credenciais:
   ```json
   {
     "email": "aluno@educaonline.com.br",
     "senha": "Teste@123"
   }
   ```
5. Clique em **"Execute"**
6. Copie o token JWT do campo `accessToken` na resposta
7. Clique no botão **"Authorize"** (cadeado) no topo da página
8. Cole o token no formato: `Bearer {seu-token-aqui}`
9. Clique em **"Authorize"** e depois **"Close"**

Agora você pode testar os endpoints protegidos! 🎉

---

### **9.2. Testando Endpoints**

Exemplos de endpoints para testar:

**Conteudo.API** (http://localhost:5105/swagger):

- `GET /api/cursos` - Listar todos os cursos
- `GET /api/cursos/{id}` - Detalhes de um curso

**Aluno.API**:

- `GET /api/alunos/{id}` - Dados do aluno
- `GET /api/alunos/{id}/matriculas` - Matrículas do aluno
- `GET /api/alunos/{id}/certificados` - Certificados emitidos

**BFF** (https://localhost:7093/swagger):

- `POST /api/matricula` - Realizar nova matrícula
- `POST /api/checkout` - Processar pagamento

---

### **9.3. Frontend Angular**

1. Acesse: http://localhost:4200
2. Faça login com:
   - Email: `aluno@educaonline.com.br`
   - Senha: `Teste@123`
3. Explore as funcionalidades:
   - Visualizar cursos disponíveis
   - Realizar matrícula
   - Acompanhar progresso
   - Visualizar certificados

---

## **10. Comunicação entre Domínios**

O **RabbitMQ** gerencia a troca de eventos assíncronos entre os serviços:

```
┌─────────────┐       UsuarioCriadoEvent       ┌─────────────┐
│ Identidade  │ ──────────────────────────────>│  Aluno.API  │
└─────────────┘                                 └─────────────┘
                                                       │
                                                       │ MatriculaCriadaEvent
                                                       ▼
┌─────────────┐      PagamentoProcessadoEvent  ┌─────────────┐
│Financeiro   │ <──────────────────────────────│ Pedidos.API │
│    .API     │                                 └─────────────┘
└─────────────┘
```

**Configuração no `appsettings.json`:**

```json
{
  "MessageQueueConnection": {
    "MessageBus": "host=localhost:5672;publisherConfirms=true;timeout=10"
  }
}
```

---

## **11. Estrutura de Arquivos do Banco de Dados**

Os arquivos SQLite são criados automaticamente em:

```
backend/src/Services/
├── EducaOnline.Identidade.API/
│   └── identidade.db               ← Criado automaticamente
├── EducaOnline.Aluno.API/
│   └── aluno.db                    ← Criado automaticamente
├── EducaOnline.Conteudo.API/
│   └── conteudo.db                 ← Criado automaticamente
├── EducaOnline.Pedidos.API/
│   └── pedidos.db                  ← Criado automaticamente
└── EducaOnline.Financeiro.API/
    └── financeiro.db               ← Criado automaticamente
```

---

## **12. Troubleshooting (Resolução de Problemas)**

### **Erro: "TaskCanceledException" ou Timeout RabbitMQ**

**Sintomas:**

```
System.Threading.Tasks.TaskCanceledException: A task was canceled.
at EasyNetQ.Persistent.PersistentChannel...
```

**Soluções:**

1. Verificar se o RabbitMQ está rodando:

   ```bash
   docker ps | grep educa-rabbit
   ```
2. Se não estiver, iniciar:

   ```bash
   docker start educa-rabbit
   ```
3. Se não existir, criar:

   ```bash
   docker run -d --hostname educa-rabbit --name educa-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```
4. **Aguardar 30-60 segundos** antes de iniciar as APIs
5. Verificar acesso: http://localhost:15672 (guest/guest)

---

### **Erro: "Port already in use"**

**Sintomas:**

```
Failed to bind to address https://localhost:7070: address already in use
```

**Soluções:**

1. **Windows - Encontrar e finalizar o processo:**

   ```powershell
   netstat -ano | findstr :7070
   taskkill /PID [número-do-pid] /F
   ```
2. **Ou alterar a porta no `launchSettings.json`:**

   ```json
   "applicationUrl": "https://localhost:NOVA_PORTA"
   ```

---

### **Erro: APIs não iniciam ou fecham imediatamente**

**Verificações:**

1. Docker Desktop está rodando?
2. RabbitMQ está ativo? (`docker ps`)
3. RabbitMQ teve tempo de inicializar? (aguardar 30s)
4. Está executando em ambiente Development?
5. O arquivo `appsettings.Development.json` existe?

---

### **Erro: "npm is not recognized"**

**Causa**: Node.js não está instalado.

**Solução**:

1. Baixe e instale: https://nodejs.org/ (versão LTS)
2. Feche e reabra todos os terminais
3. Verifique: `node --version` e `npm --version`

---

### **Warnings CS8618 (nullable references)**

**Exemplo:**

```
warning CS8618: Non-nullable property 'Title' must contain a non-null value
```

**Causa**: Avisos de compilação do C# 9+ sobre nullability.

**Impacto**: Não impedem a execução do projeto.

**Solução** (opcional):

```csharp
public required string Title { get; set; }  
public string? Title { get; set; }        
public string Title { get; set; } = "";  
```

---

### **🔄 Reset Completo do Projeto**

Se nada funcionar, execute um reset completo:

```powershell
# 1. Parar e remover RabbitMQ
docker stop educa-rabbit
docker rm educa-rabbit

# 2. Deletar todos os bancos de dados
Remove-Item "backend\src\Services\*\*.db" -Force

# 3. Limpar node_modules
Remove-Item "frontend\node_modules" -Recurse -Force

# 4. Recriar RabbitMQ
docker run -d --hostname educa-rabbit --name educa-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# 5. Aguardar 30 segundos
Start-Sleep -Seconds 30

# 6. Reinstalar dependências do frontend
cd frontend
npm install

# 7. Executar as APIs novamente (na ordem correta)
```

---

## **13. Documentação Técnica**

### **Padrões Arquiteturais**

- **DDD** (Domain-Driven Design)
- **CQRS** com MediatR
- **Event-Driven Architecture**
- **Clean Architecture**
- **Repository + Unit of Work Pattern**
- **Value Objects** e **Entidades Ricas**

### **Tecnologias Backend**

- **.NET 9.0**
- **ASP.NET Core Identity** + **JWT Bearer**
- **Entity Framework Core** + **SQLite**
- **RabbitMQ** + **EasyNetQ**
- **AutoMapper**
- **FluentValidation**
- **Dapper** (queries otimizadas)
- **MediatR** (CQRS)

### **Tecnologias Frontend**

- **Angular 17+**
- **Nx Monorepo**
- **TypeScript**
- **RxJS**

### **Infraestrutura**

- **Docker** (RabbitMQ)
- **SQLite** (Development)

---

## **14. Estrutura de Camadas (por microserviço)**

```
API Layer
   ├── Controllers        → Exposição de endpoints REST
   └── Configuration      → Setup de serviços

Application Layer
   ├── Commands          → Ações de escrita (CQRS)
   ├── Queries           → Ações de leitura (CQRS)
   ├── Handlers          → Processadores (MediatR)
   └── ViewModels        → DTOs de resposta

Domain Layer
   ├── Entities          → Agregados e entidades ricas
   ├── ValueObjects      → Objetos de valor imutáveis
   ├── Events            → Eventos de domínio
   └── Interfaces        → Contratos

Infrastructure Layer
   ├── Data              → DbContext (EF Core)
   ├── Repositories      → Implementação de repositórios
   └── Integrations      → Message Bus, APIs externas
```

---

## **15. Checklist de Validação**

Antes de reportar problemas, verifique:

```
[ ] Docker Desktop está rodando
[ ] Container RabbitMQ está ativo (docker ps)
[ ] RabbitMQ está acessível (http://localhost:15672)
[ ] Node.js e npm estão instalados
[ ] .NET SDK 9.0 está instalado
[ ] Arquivos .db foram criados nas pastas dos serviços
[ ] APIs foram iniciadas na ordem correta
[ ] Aguardou 30s após iniciar RabbitMQ
[ ] Swagger das APIs está acessível
[ ] Login funciona (aluno@educaonline.com.br / Teste@123)
[ ] Token JWT é gerado corretamente
[ ] Frontend carrega em http://localhost:4200
```

---

## **16. Próximos Passos**

Após iniciar todos os serviços com sucesso:

1. Explore o Swagger de cada API
2. Teste o fluxo completo:
   - Login como aluno
   - Visualizar cursos disponíveis
   - Realizar matrícula
   - Processar pagamento
   - Acompanhar progresso
   - Emitir certificado
3. Analise os eventos no RabbitMQ Management
4. Explore o código-fonte para entender a arquitetura

---

## **17. Melhorias Futuras (Sugestões)**

### **Curto Prazo:**

- [ ] Docker Compose para orquestração de serviços
- [ ] Health checks em cada API
- [ ] Script de setup automático

### **Médio Prazo:**

- [ ] Migração para PostgreSQL/SQL Server
- [ ] Implementação de Circuit Breaker (Polly)
- [ ] Logs estruturados (Serilog)
- [ ] Distributed Tracing (OpenTelemetry)
- [ ] Testes unitários e de integração

---

## **18. Contato e Suporte**

Este projeto é parte do **MBA DevXpert Full Stack .NET** e é mantido pelos autores para fins acadêmicos.

Para dúvidas ou sugestões:

- Entre em contato com os autores
- Abra uma Issue no repositório (se disponível)

---

## **19. Licença**

Projeto acadêmico - Todos os direitos reservados aos autores.

---

## **Conclusão**

O **EducaOnline** demonstra a aplicação prática de conceitos modernos de arquitetura de software, incluindo microsserviços, DDD, CQRS e event-driven architecture, em um contexto educacional realista.

**Boa exploração do projeto!**

---

**Última atualização**: Novembro/2024
