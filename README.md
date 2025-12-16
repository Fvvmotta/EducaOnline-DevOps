# EducaOnline - Plataforma Modular de Gestao de Cursos e Alunos

## 1. Visao Geral

O **EducaOnline** e uma plataforma corporativa distribuida, construida com **arquitetura de microsservicos**, para **gestao completa de cursos, alunos, matriculas, pagamentos e certificados**, integrando multiplos dominios via **mensageria RabbitMQ**, um **BFF (Backend for Frontend)** em .NET e um **frontend em Angular**.

O projeto foi desenvolvido como parte do MBA **DevXpert Full Stack .NET**, nos modulos **Construcao de Aplicacoes Corporativas** e **DevOps e Cloud**, aplicando **DDD (Domain-Driven Design)**, **CQRS**, **Event-Driven Architecture**, **Clean Architecture** e praticas modernas de **DevOps**.

---

## 2. Autor

- **Fernando Vinicius Valim Motta**

---

## 3. Arquitetura do Projeto

```
EducaOnline/
|
+-- .github/
|   +-- workflows/
|       +-- ci-cd.yml                     -> Pipeline CI/CD principal
|       +-- pr-validation.yml             -> Validacao de Pull Requests
|
+-- backend/
|   +-- src/
|       +-- api_gateways/
|       |   +-- EducaOnline.Bff/          -> BFF central que integra os dominios
|       +-- building_blocks/
|       |   +-- EducaOnline.Core/         -> Dominios compartilhados, validacoes
|       |   +-- EducaOnline.MessageBus/   -> Implementacao do barramento RabbitMQ
|       |   +-- EducaOnline.WebAPI.Core/  -> Middlewares, JWT, Health Checks
|       +-- services/
|           +-- EducaOnline.Aluno.API/    -> Contexto de alunos e matriculas
|           +-- EducaOnline.Conteudo.API/ -> Contexto de cursos e aulas
|           +-- EducaOnline.Financeiro.API/ -> Contexto de pagamentos
|           +-- EducaOnline.Identidade.API/ -> Autenticacao JWT
|           +-- EducaOnline.Pedidos.API/  -> Contexto de pedidos
|
+-- frontend/                             -> Aplicacao Angular com Nx
|   +-- Dockerfile                        -> Build multi-stage do frontend
|   +-- nginx.conf                        -> Configuracao do servidor web
|
+-- k8s/
|   +-- base/                             -> ConfigMaps, Secrets, Namespace
|   +-- services/                         -> Deployments e Services K8s
|
+-- docker-compose.yml                    -> Orquestracao local
+-- README.md
```

---

## 4. Infraestrutura DevOps

### 4.1 Docker

Cada microsservico possui seu proprio **Dockerfile** multi-stage otimizado:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# ... compilacao

# Runtime stage (imagem final ~10x menor)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
```

O frontend utiliza **Node.js** para build e **Nginx** para servir a aplicacao:

```dockerfile
# Build stage
FROM node:20-alpine AS build
# ... npm ci && nx build

# Runtime stage
FROM nginx:alpine AS final
```

**Imagens publicadas no Docker Hub:**

Backend:
- fermotta/educaonline-identidade-api:latest
- fermotta/educaonline-conteudo-api:latest
- fermotta/educaonline-aluno-api:latest
- fermotta/educaonline-pedidos-api:latest
- fermotta/educaonline-financeiro-api:latest
- fermotta/educaonline-bff:latest

Frontend:
- fermotta/educaonline-frontend:latest

### 4.2 Docker Compose

Para executar localmente com todos os servicos:

```bash
# Subir toda a infraestrutura
docker-compose up -d

# Verificar status
docker-compose ps

# Ver logs de um servico
docker-compose logs -f identidade-api

# Parar tudo
docker-compose down
```

**Servicos e Portas:**

| Servico | Porta | URL |
|---------|-------|-----|
| Frontend Angular | 4200 | http://localhost:4200 |
| BFF | 5000 | http://localhost:5000/swagger |
| Identidade API | 5001 | http://localhost:5001/swagger |
| Conteudo API | 5002 | http://localhost:5002/swagger |
| Aluno API | 5003 | http://localhost:5003/swagger |
| Pedidos API | 5004 | http://localhost:5004/swagger |
| Financeiro API | 5005 | http://localhost:5005/swagger |
| RabbitMQ | 15672 | http://localhost:15672 (guest/guest) |

### 4.3 Health Checks

Todas as APIs expoem endpoints de health check para monitoramento:

| Endpoint | Descricao |
|----------|-----------|
| /health | Status completo com todos os checks |
| /health/ready | Readiness probe (para Kubernetes) |
| /health/live | Liveness probe (verificacao simples) |

Exemplo de resposta:
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

### 4.4 CI/CD com GitHub Actions

O pipeline e executado automaticamente em pushes para main e develop:

**Jobs do Pipeline:**
1. build-and-test-backend - Compilacao e testes do backend .NET
2. build-and-test-frontend - Compilacao do frontend Angular
3. code-analysis - Verificacao de formatacao de codigo
4. docker-build-push-backend - Build e push das imagens backend
5. docker-build-push-frontend - Build e push da imagem frontend
6. notify-success - Notificacao de sucesso

**Secrets necessarios no GitHub:**
- DOCKERHUB_USERNAME - Usuario do Docker Hub
- DOCKERHUB_TOKEN - Token de acesso do Docker Hub

### 4.5 Kubernetes

**Pre-requisitos:**
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
- Namespace: educaonline
- ConfigMap: configuracoes compartilhadas
- Secret: credenciais sensiveis
- Deployments: 2 replicas por servico
- Services: ClusterIP (APIs) e LoadBalancer (BFF)
- Health Probes: Liveness e Readiness

---

## 5. Pre-requisitos

### Obrigatorios:

| Ferramenta | Versao | Download |
|------------|--------|----------|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download |
| Node.js | 20+ LTS | https://nodejs.org/ |
| Docker Desktop | Latest | https://www.docker.com/products/docker-desktop |
| Git | Latest | https://git-scm.com/ |

### Opcionais:

- Visual Studio 2022
- Visual Studio Code
- kubectl (para Kubernetes)
- Kind ou Minikube (para Kubernetes local)

---

## 6. Executando o Projeto

### Opcao 1: Docker Compose (Recomendado)

```bash
# Clonar o repositorio
git clone https://github.com/Fvvmotta/EducaOnline-DevOps.git
cd EducaOnline-DevOps

# Subir todos os servicos
docker-compose up -d

# Aguardar os servicos iniciarem (30-60 segundos)
docker-compose ps

# Acessar a aplicacao
# http://localhost:4200 (Frontend)
# http://localhost:5000/swagger (BFF)
# http://localhost:5001/swagger (Identidade)
```

### Opcao 2: Execucao Local (Desenvolvimento)

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

# 4. Executar o frontend
cd frontend
npm install
npx nx serve educa-online
```

### Opcao 3: Kubernetes

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

## 7. Dados Iniciais (Seed)

Em ambiente Development, usuarios padrao sao criados:

**Administrador:**
- Email: admin@educaonline.com.br
- Senha: Teste@123

**Aluno:**
- Email: aluno@educaonline.com.br
- Senha: Teste@123

---

## 8. Tecnologias

### Backend
- .NET 9.0
- ASP.NET Core Identity + JWT
- Entity Framework Core + SQLite
- RabbitMQ + EasyNetQ
- AutoMapper, MediatR, FluentValidation

### Frontend
- Angular 20
- Nx Monorepo
- Angular Material
- TypeScript, RxJS
- Nginx (producao)

### DevOps
- Docker e Docker Compose
- GitHub Actions (CI/CD)
- Kubernetes
- Health Checks

---

## 9. Estrutura de Health Checks

Os health checks foram implementados em EducaOnline.WebAPI.Core/Configuration/HealthCheckExtensions.cs:

```csharp
// Configuracao
services.AddHealthCheckConfig(configuration);

// Uso
app.UseHealthCheckConfig();
```

---

## 10. Troubleshooting

### Containers reiniciando (CrashLoopBackOff)

```bash
# Ver logs do container
docker-compose logs -f nome-do-servico
kubectl logs -n educaonline deployment/nome-do-servico
```

### Erro de conexao RabbitMQ
- Verificar se o RabbitMQ esta rodando
- Aguardar 30-60 segundos apos iniciar

### Health check retornando 404
- Verificar se AddHealthCheckConfig e UseHealthCheckConfig estao configurados
- Verificar se o arquivo HealthCheckExtensions.cs existe

### Frontend nao carrega
- Verificar se o BFF esta rodando
- Verificar logs do container frontend: docker-compose logs -f frontend

---

## 11. Links Uteis

- Docker Hub: https://hub.docker.com/u/fermotta
- GitHub: https://github.com/Fvvmotta/EducaOnline-DevOps
- GitHub Actions: https://github.com/Fvvmotta/EducaOnline-DevOps/actions

---

## 12. Licenca

Projeto academico - MBA DevXpert Full Stack .NET

---

Ultima atualizacao: Dezembro/2025