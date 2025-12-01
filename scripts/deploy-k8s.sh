#!/bin/bash

# ============================================
# Script de Deploy - EducaOnline Kubernetes
# ============================================

set -e

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  EducaOnline - Deploy Kubernetes${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Verificar se kubectl está instalado
if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}❌ kubectl não encontrado. Por favor, instale o kubectl.${NC}"
    exit 1
fi

# Verificar conexão com cluster
echo -e "${YELLOW}📡 Verificando conexão com cluster Kubernetes...${NC}"
if ! kubectl cluster-info &> /dev/null; then
    echo -e "${RED}❌ Não foi possível conectar ao cluster Kubernetes.${NC}"
    echo -e "${YELLOW}Dica: Inicie o Kind com: kind create cluster --name educaonline${NC}"
    exit 1
fi
echo -e "${GREEN}✅ Conectado ao cluster${NC}"
echo ""

# Aplicar namespace
echo -e "${YELLOW}📦 Criando namespace...${NC}"
kubectl apply -f k8s/base/namespace.yaml
echo -e "${GREEN}✅ Namespace criado${NC}"

# Aplicar ConfigMap e Secrets
echo -e "${YELLOW}🔐 Aplicando configurações...${NC}"
kubectl apply -f k8s/base/configmap.yaml
kubectl apply -f k8s/base/secrets.yaml
echo -e "${GREEN}✅ Configurações aplicadas${NC}"

# Aplicar infraestrutura
echo -e "${YELLOW}🏗️  Iniciando infraestrutura...${NC}"
kubectl apply -f k8s/base/rabbitmq.yaml
kubectl apply -f k8s/base/sqlserver.yaml

echo -e "${YELLOW}⏳ Aguardando RabbitMQ ficar pronto...${NC}"
kubectl wait --for=condition=ready pod -l app=rabbitmq -n educaonline --timeout=120s || true

echo -e "${YELLOW}⏳ Aguardando SQL Server ficar pronto...${NC}"
kubectl wait --for=condition=ready pod -l app=sqlserver -n educaonline --timeout=180s || true
echo -e "${GREEN}✅ Infraestrutura pronta${NC}"

# Aplicar microsserviços
echo -e "${YELLOW}🚀 Iniciando microsserviços...${NC}"
kubectl apply -f k8s/services/

# Aguardar pods ficarem prontos
echo -e "${YELLOW}⏳ Aguardando microsserviços ficarem prontos...${NC}"
sleep 10

kubectl wait --for=condition=ready pod -l app=identidade-api -n educaonline --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=conteudo-api -n educaonline --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=aluno-api -n educaonline --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=pedidos-api -n educaonline --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=financeiro-api -n educaonline --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=bff -n educaonline --timeout=120s || true

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  ✅ Deploy concluído com sucesso!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Mostrar status
echo -e "${YELLOW}📊 Status dos pods:${NC}"
kubectl get pods -n educaonline

echo ""
echo -e "${YELLOW}📊 Status dos services:${NC}"
kubectl get services -n educaonline

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}  Para acessar o BFF:${NC}"
echo -e "${GREEN}  kubectl port-forward svc/bff-service 8080:80 -n educaonline${NC}"
echo -e "${GREEN}  Acesse: http://localhost:8080/swagger${NC}"
echo -e "${GREEN}========================================${NC}"
