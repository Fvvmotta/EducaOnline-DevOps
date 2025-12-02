# **EducaOnline – Plataforma Modular de Gestão de Cursos e Alunos**

## **1. Visão Geral**

O **EducaOnline** é uma plataforma corporativa distribuída, construída com **arquitetura de microsserviços**, para **gestão completa de cursos, alunos, matrículas, pagamentos e certificados**, integrando múltiplos domínios via **mensageria RabbitMQ** e um **BFF (Backend for Frontend)** em .NET e um **frontend em Angular**.

O projeto foi desenvolvido como parte do MBA **DevXpert Full Stack .NET**, nos módulos **Construção de Aplicações Corporativas** e **DevOps e Cloud**, aplicando **DDD (Domain-Driven Design)**, **CQRS**, **Event-Driven Architecture**, **Clean Architecture** e práticas modernas de **DevOps**.

---

## **2. Autor**

- **Fernando Vinícius Valim Motta**

---

## **3. Arquitetura do Projeto**

A solução é organizada em **camadas independentes**:

```
EducaOnline/
│
├── .github/
│   └── workflows/
│       ├── ci-cd.yml                     → Pipeline CI/CD principal
│       └── pr-validation.yml             → Validação de Pull Requests
│
├── backend/
│   └── src/
│       ├── api_gateways/
│       │   └── EducaOnline.Bff/          → BFF central que integra os domínios
│       ├── building_blocks/
│       │   ├── EducaOnline.Core/         → Domínios compartilhados, validações
│       │   ├── EducaOnline.MessageBus/   → Implementação do barramento RabbitMQ
│       │   └── EducaOnline.WebAPI.Core/  → Middlewares, JWT, Health Checks
│       └── services/
│           ├── EducaOnline.Aluno.API/    → Contexto de alunos e matrículas
│           ├── EducaOnline.Conteudo.API/ → Contexto de cursos e aulas
│           ├── EducaOnline.Financeiro.API/→ Contexto de pagamentos
│           ├── EducaOnline.Identidade.API/→ Autenticação JWT
│           └── EducaOnline.Pedidos.API/  → Contexto de pedidos
│
├── k8s/
│   ├── base/                             → ConfigMaps, Secrets, Namespace
│   └── services/                         → Deployments e Services K8s
│
├── scripts/
│   ├── deploy-k8s.sh                     → Script de deploy Kubernetes
│   └── cleanup-k8s.sh                    → Script de limpeza
│
├── frontend/                             → Aplicação Angular
│
├── docker-compose.yml                    → Orquestração local
└── README.md
```

---

## **4. Infraestrutura DevOps**

### **4.1 Docker**

Cada microsserviço possui seu próprio **Dockerfile** multi-stage otimizado:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# ... compilação

# Runtime stage (imagem final ~10x menor)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
```

**Imagens publicadas no Docker Hub:**
- `fermotta/educaonline-identidade-api:latest`
- `fermotta/educaonline-conteudo-api:latest`
- `fermotta/educaonline-aluno-api:latest`
- `fermotta/educaonline-pedidos-api:latest`
- `fermotta/educaonline-financeiro-api:latest`
- `fermotta/educaonline-bff:latest`

### **4.2 Docker Compose**

Para executar localmente com todos os serviços:

```bash
# Subir toda a infraestrutura
docker-compose up -d

# Verificar status
docker-compose ps

# Ver logs de um serviço
docker-compose logs -f identidade-api

# Parar tudo
docker-compose down
```

**Serviços e Portas (Docker Compose):**

| Serviço | Porta | URL |
|---------|-------|-----|
| BFF | 5000 | http://localhost:5000/swagger |
| Identidade API | 5001 | http://localhost:5001/swagger |
| Conteudo API | 5002 | http://localhost:5002/swagger |
| Aluno API | 5003 | http://localhost:5003/swagger |
| Pedidos API | 5004 | http://localhost:5004/swagger |
| Financeiro API | 5005 | http://localhost:5005/swagger |
| RabbitMQ | 15672 | http://localhost:15672 (guest/guest) |

### **4.3 Health Checks**

Todas as APIs expõem endpoints de health check para monitoramento:

| Endpoint | Descrição |
|----------|-----------|
| `/health` | Status completo com todos os checks |
| `/health/ready` | Readiness probe (para Kubernetes) |
| `/health/live` | Liveness probe (verificação simples) |

**Exemplo de resposta:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-12-01T20:00:00Z",
  "duration": 15.5,
  "checks": [
    {
      "name": "self",
      "status": "Healthy",
      "description": "API is running"
    }
  ]
}
```

### **4.4 CI/CD com GitHub Actions**

O pipeline é executado automaticamente em pushes para `main` e `develop`:

**Jobs do Pipeline:**
1. **build-and-test** - Compilação e execução de testes
2. **code-analysis** - Verificação de formatação de código
3. **docker-build-push** - Build e push das imagens para Docker Hub
4. **notify-success** - Notificação de sucesso

**Secrets necessários no GitHub:**
- `DOCKERHUB_USERNAME` - Usuário do Docker Hub
- `DOCKERHUB_TOKEN` - Token de acesso do Docker Hub

### **4.5 Kubernetes**

**Pré-requisitos:**
- Docker Desktop com Kubernetes habilitado, ou
- Kind (Kubernetes in Docker), ou
- Minikube

**Deploy no Kubernetes:**

```bash
# Aplicar todos os manifests
kubectl apply -f k8s/base/namespace.yaml
kubectl apply -f k8s/base/
kubectl apply -f k8s/services/

# Verificar pods
kubectl get pods -n educaonline

# Verificar services
kubectl get svc -n educaonline

# Ver logs de um pod
kubectl logs -n educaonline deployment/identidade-api

# Deletar tudo
kubectl delete namespace educaonline
```

**Recursos Kubernetes configurados:**
- Namespace: `educaonline`
- ConfigMap: configurações compartilhadas
- Secret: credenciais sensíveis
- Deployments: 2 réplicas por serviço
- Services: ClusterIP (APIs) e LoadBalancer (BFF)
- Health Probes: Liveness e Readiness

---

## **5. Pré-requisitos**

### **Obrigatórios:**

| Ferramenta | Versão | Download |
|------------|--------|----------|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download |
| Node.js | 20+ LTS | https://nodejs.org/ |
| Docker Desktop | Latest | https://www.docker.com/products/docker-desktop |
| Git | Latest | https://git-scm.com/ |

### **Opcionais:**

- Visual Studio 2022
- Visual Studio Code
- kubectl (para Kubernetes)
- Kind ou Minikube (para Kubernetes local)

---

## **6. Executando o Projeto**

### **Opção 1: Docker Compose (Recomendado)**

```bash
# Clonar o repositório
git clone https://github.com/Fvvmotta/EducaOnline-DevOps.git
cd EducaOnline-DevOps

# Subir todos os serviços
docker-compose up -d

# Aguardar os serviços iniciarem (30-60 segundos)
docker-compose ps

# Acessar as APIs
# http://localhost:5000/swagger (BFF)
# http://localhost:5001/swagger (Identidade)
```

### **Opção 2: Execução Local (Desenvolvimento)**

```bash
# 1. Iniciar RabbitMQ
docker run -d --name educa-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# 2. Aguardar 30 segundos

# 3. Executar cada API em terminais separados
cd backend/src/services/EducaOnline.Identidade.API && dotnet run
cd backend/src/services/EducaOnline.Conteudo.API && dotnet run
cd backend/src/services/EducaOnline.Aluno.API && dotnet run
cd backend/src/services/EducaOnline.Pedidos.API && dotnet run
cd backend/src/services/EducaOnline.Financeiro.API && dotnet run
cd backend/src/api_gateways/EducaOnline.Bff && dotnet run
```

### **Opção 3: Kubernetes**

```bash
# Habilitar Kubernetes no Docker Desktop ou usar Kind
kind create cluster --name educaonline

# Aplicar manifests
kubectl apply -f k8s/base/namespace.yaml
kubectl apply -f k8s/base/
kubectl apply -f k8s/services/

# Verificar status
kubectl get pods -n educaonline
```

---

## **7. Dados Iniciais (Seed)**

Em ambiente **Development**, usuários padrão são criados:

**Administrador:**
- Email: `admin@educaonline.com.br`
- Senha: `Teste@123`

**Aluno:**
- Email: `aluno@educaonline.com.br`
- Senha: `Teste@123`

---

## **8. Tecnologias**

### **Backend**
- .NET 9.0
- ASP.NET Core Identity + JWT
- Entity Framework Core + SQLite
- RabbitMQ + EasyNetQ
- AutoMapper, MediatR, FluentValidation

### **Frontend**
- Angular 17+
- Nx Monorepo
- TypeScript, RxJS

### **DevOps**
- Docker & Docker Compose
- GitHub Actions (CI/CD)
- Kubernetes
- Health Checks

---

## **9. Estrutura de Health Checks**

Os health checks foram implementados em `EducaOnline.WebAPI.Core/Configuration/HealthCheckExtensions.cs`:

```csharp
// Configuração
services.AddHealthCheckConfig(configuration);

// Uso
app.UseHealthCheckConfig();
```

---

## **10. Troubleshooting**

### **Containers reiniciando (CrashLoopBackOff)**

```bash
# Ver logs do container
docker-compose logs -f nome-do-servico
kubectl logs -n educaonline deployment/nome-do-servico
```

### **Erro de conexão RabbitMQ**
- Verificar se o RabbitMQ está rodando
- Aguardar 30-60 segundos após iniciar

### **Health check retornando 404**
- Verificar se `AddHealthCheckConfig` e `UseHealthCheckConfig` estão configurados
- Verificar se o arquivo `HealthCheckExtensions.cs` existe

---

## **11. Links Úteis**

- **Docker Hub**: https://hub.docker.com/u/fermotta
- **GitHub**: https://github.com/Fvvmotta/EducaOnline-DevOps
- **GitHub Actions**: https://github.com/Fvvmotta/EducaOnline-DevOps/actions

---

## **12. Licença**

Projeto acadêmico - MBA DevXpert Full Stack .NET

---

**Última atualização**: Dezembro/2025
