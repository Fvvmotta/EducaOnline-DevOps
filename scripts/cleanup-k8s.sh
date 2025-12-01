#!/bin/bash

# ============================================
# Script de Cleanup - EducaOnline Kubernetes
# ============================================

set -e

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}========================================${NC}"
echo -e "${YELLOW}  EducaOnline - Cleanup Kubernetes${NC}"
echo -e "${YELLOW}========================================${NC}"
echo ""

# Verificar se kubectl está instalado
if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}❌ kubectl não encontrado.${NC}"
    exit 1
fi

# Deletar namespace (remove todos os recursos)
echo -e "${YELLOW}🗑️  Removendo namespace educaonline...${NC}"
kubectl delete namespace educaonline --ignore-not-found=true

echo ""
echo -e "${GREEN}✅ Cleanup concluído!${NC}"
echo ""

# Verificar se Kind cluster deve ser removido
read -p "Deseja também remover o cluster Kind? (s/N): " -n 1 -r
echo
if [[ $REPLY =~ ^[Ss]$ ]]; then
    echo -e "${YELLOW}🗑️  Removendo cluster Kind...${NC}"
    kind delete cluster --name educaonline 2>/dev/null || true
    echo -e "${GREEN}✅ Cluster Kind removido${NC}"
fi
