# Usuarios

**Nome em Inglês:** Fiap Cloud Games
**Nome em Português:** Fiap Jogos na nuvem

## Integrantes do Grupo

- Saulo Szmyhiel Ganança
- Leonardo Bernardes
- Rodrigo Ferreira
- Renato Ventura

## Descrição

Este é um projeto de pós-graduação em Arquitetura de Software, desenvolvido pelo Grupo 127 da FIAP. O **Microservico de usuários ** tem como objetivo fornecer uma estrutura moderna e escalável para vendas de jogos e gestão de servidores para partidas online, aplicando os principais conceitos de arquitetura de software, boas práticas de desenvolvimento e padrões de mercado.

## Tecnologias Utilizadas

- .NET 8
- C# 12
- DDD (Domain-Driven Design)
- TDD (Test-Driven Development)
- BDD (Behavior-Driven Development)
- PostgreSQL (AWS RDS)
- Docker
- Kubernetes (Amazon EKS)
- AWS Cloud (EKS, RDS, ECR, VPC, LoadBalancer)

## Conteúdo do Projeto

### **Fase 3**

- Novidades do .NET 8 e C# 12
- Criação de API REST com .NET
- Middlewares e Injeção de Dependência
- Implementação de Logs
- Serialização com JSON e MessagePack
- Autenticação e Autorização
- Swagger e documentação de API
- Implementação de Cache

## Estrutura do Projeto

A estrutura inicial segue o padrão **DDD (Domain-Driven Design)**, dividida em camadas:

```
src/
├── Usuarios.API             → Camada de apresentação (API REST)
├── Usuarios.Application     → Casos de uso e lógica de aplicação
├── Usuarios.Domain          → Entidades de domínio e contratos
└── Usuarios.Infrastructure  → Implementações de persistência e serviços externos
```

Essa organização promove separação de responsabilidades, facilitando testes, manutenibilidade e escalabilidade.

## Arquitetura AWS

O projeto está hospedado na **AWS (Amazon Web Services)** com a seguinte arquitetura:

### **Recursos Utilizados**

- **Amazon EKS (Elastic Kubernetes Service)**: Cluster Kubernetes gerenciado para orquestração de containers
- **Amazon RDS (PostgreSQL 15)**: Banco de dados gerenciado com alta disponibilidade
- **Amazon ECR (Elastic Container Registry)**: Repositório privado de imagens Docker
- **AWS VPC**: Rede virtual isolada com subnets públicas e privadas
- **AWS LoadBalancer**: Network Load Balancer para acesso externo à API

### **Infraestrutura**

```
┌─────────────────────────────────────────────────┐
│                  AWS Cloud                       │
│  ┌───────────────────────────────────────────┐  │
│  │            VPC (us-east-1)                │  │
│  │  ┌─────────────────┐  ┌────────────────┐ │  │
│  │  │   EKS Cluster   │  │   RDS Instance │ │  │
│  │  │   users-cluster │  │   PostgreSQL   │ │  │
│  │  │                 │  │   users_db     │ │  │
│  │  │  ┌───────────┐  │  └────────────────┘ │  │
│  │  │  │  Pod 1    │  │         ↑           │  │
│  │  │  │  users-api│  │         │           │  │
│  │  │  └───────────┘  │         │           │  │
│  │  │  ┌───────────┐  │    Connection       │  │
│  │  │  │  Pod 2    │──┼─────────┘           │  │
│  │  │  │  users-api│  │                     │  │
│  │  │  └───────────┘  │                     │  │
│  │  └─────────────────┘                     │  │
│  └───────────────────────────────────────────┘  │
│           ↑                                      │
│    LoadBalancer (NLB)                            │
└───────────┼────────────────────────────────────┘
            │
        Internet
```

### **Configuração de Rede**

- **Region**: us-east-1
- **VPC CIDR**: 10.0.0.0/16
- **Subnets Públicas**: 2 subnets em diferentes AZs
- **Security Groups**: Configurados para permitir tráfego entre EKS e RDS na porta 5432

## Deploy na AWS

### **Pré-requisitos**

- [AWS CLI](https://aws.amazon.com/cli/)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [Docker](https://www.docker.com/products/docker-desktop/)
- Conta AWS (AWS Academy Lab ou conta pessoal)

### **1. Configurar Credenciais AWS**

Para AWS Academy Lab, copie as credenciais do Lab Details:

```bash
cat > ~/.aws/credentials << 'EOF'
[default]
aws_access_key_id=YOUR_ACCESS_KEY
aws_secret_access_key=YOUR_SECRET_KEY
aws_session_token=YOUR_SESSION_TOKEN
EOF
```

### **2. Criar Recursos AWS**

#### **EKS Cluster**

```bash
aws eks create-cluster \
  --name users-cluster \
  --role-arn arn:aws:iam::211125430602:role/LabRole \
  --resources-vpc-config subnetIds=subnet-xxx,subnet-yyy,securityGroupIds=sg-xxx \
  --region us-east-1

# Aguardar criação (15-20 minutos)
aws eks wait cluster-active --name users-cluster --region us-east-1

# Configurar kubectl
aws eks update-kubeconfig --region us-east-1 --name users-cluster
```

#### **RDS PostgreSQL**

```bash
aws rds create-db-instance \
  --db-instance-identifier users-db \
  --db-instance-class db.t3.micro \
  --engine postgres \
  --engine-version 15.8 \
  --master-username postgres \
  --master-user-password YOUR_SECURE_PASSWORD \
  --allocated-storage 20 \
  --vpc-security-group-ids sg-xxx \
  --db-subnet-group-name your-subnet-group \
  --publicly-accessible \
  --region us-east-1
```

#### **ECR Repository**

```bash
aws ecr create-repository \
  --repository-name users-api \
  --region us-east-1
```

### **3. Configurar GitHub Secrets**

No repositório GitHub, adicione os seguintes secrets (Settings → Secrets and variables → Actions):

- `AWS_ACCESS_KEY_ID`: Sua AWS Access Key
- `AWS_SECRET_ACCESS_KEY`: Sua AWS Secret Key
- `AWS_SESSION_TOKEN`: Seu AWS Session Token (para AWS Academy)
- `DB_HOST`: Endpoint do RDS (ex: `users-db.xxx.us-east-1.rds.amazonaws.com`)
- `DB_NAME`: Nome do banco (`users_db`)
- `DB_USER`: Usuário do banco (`postgres`)
- `DB_PASSWORD`: Senha do banco
- `JWT_KEY`: Chave secreta para JWT (mínimo 32 caracteres)

### **4. Deploy Automático via GitHub Actions**

O projeto possui CI/CD configurado. Basta fazer push para a branch `main`:

```bash
git add .
git commit -m "feat: deploy to AWS"
git push origin main
```

O workflow `.github/workflows/deploy_aws.yml` irá:

1. Buildar a aplicação
2. Executar os testes
3. Criar imagem Docker otimizada (Alpine)
4. Fazer push para o ECR
5. Aplicar manifests Kubernetes
6. Deployar a aplicação no EKS

### **5. Acessar a Aplicação**

Obter a URL do LoadBalancer:

```bash
kubectl get service users-service -n users
```

Acessar o Swagger:

```
http://EXTERNAL-IP/swagger
```

### **6. Verificar Status**

```bash
# Verificar pods
kubectl get pods -n users

# Verificar logs
kubectl logs -f -n users -l app=users-api

# Verificar deployment
kubectl get deployment users-api -n users

# Verificar service
kubectl get service users-service -n users
```

## Gerenciamento de Secrets

### **Criar/Atualizar Secrets no Kubernetes**

```bash
# Namespace
kubectl create namespace users --dry-run=client -o yaml | kubectl apply -f -

# Secrets
kubectl create secret generic users-secret \
  --from-literal=ConnectionStrings__PostgreSql="Host=YOUR_RDS_ENDPOINT;Database=users_db;Username=postgres;Password=YOUR_PASSWORD" \
  --from-literal=ConnectionStrings__DefaultConnection="Host=YOUR_RDS_ENDPOINT;Database=users_db;Username=postgres;Password=YOUR_PASSWORD" \
  --from-literal=Jwt__Key="YOUR_JWT_KEY_MIN_32_CHARS" \
  --namespace=users \
  --dry-run=client -o yaml | kubectl apply -f -
```

### **Verificar Secrets**

```bash
# Listar secrets
kubectl get secrets -n users

# Ver conteúdo (base64 encoded)
kubectl get secret users-secret -n users -o yaml

# Decodificar um valor específico
kubectl get secret users-secret -n users -o jsonpath='{.data.Jwt__Key}' | base64 -d
```

## Acesso ao Banco de Dados

### **Via kubectl exec (psql)**

```bash
# Conectar ao pod
kubectl exec -it deployment/users-api -n users -- sh

# Dentro do pod
apk add --no-cache postgresql-client
psql "postgresql://postgres:PASSWORD@RDS_ENDPOINT:5432/users_db"
```

### **Consultas SQL Úteis**

```sql
-- Listar todos os usuários
SELECT "Id", "Name", "Email", "AccessLevel", "CreatedAt" 
FROM "Users" 
ORDER BY "CreatedAt" DESC;

-- Atualizar senha de um usuário
UPDATE "Users" 
SET "Password" = 'nova_senha_hash', "UpdatedAt" = NOW() 
WHERE "Email" = 'usuario@example.com';

-- Verificar nível de acesso
SELECT "Name", "Email", 
  CASE 
    WHEN "AccessLevel" = 1 THEN 'User'
    WHEN "AccessLevel" = 2 THEN 'Admin'
    ELSE 'Unknown'
  END as "Role"
FROM "Users";
```

## Dockerfile Otimizado

O projeto utiliza Docker multi-stage build com Alpine Linux para minimizar o tamanho da imagem:

**Otimizações:**

- ✅ Imagem base Alpine (~70% menor que Debian)
- ✅ Multi-stage build (SDK separado do runtime)
- ✅ Usuário não-root para segurança
- ✅ Porta 8080 (não privilegiada)
- ✅ Cache de layers otimizado
- ✅ Dependências ICU instaladas para globalização

**Tamanho da imagem**: ~55-60MB (vs ~200MB com imagem padrão)

## Como rodar o projeto

### **Desenvolvimento Local**

#### **Pré-requisitos**

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [PostgreSQL](https://www.postgresql.org/download/) ou Docker

#### **Banco de Dados Local com Docker**

```bash
docker run --name postgres-local \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=users_db \
  -p 5432:5432 \
  -d postgres:15-alpine
```

#### **Configurar appsettings.Development.json**

```json
{
  "ConnectionStrings": {
    "PostgreSql": "Host=localhost;Database=users_db;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters-long"
  }
}
```

#### **Executar Migrations**

```bash
dotnet ef migrations add InitialCreate --project src/Usuarios.Infrastructure --startup-project src/Usuarios.API
dotnet ef database update --project src/Usuarios.Infrastructure --startup-project src/Usuarios.API
```

#### **Rodar a Aplicação**

```bash
cd src/Usuarios.API
dotnet run
```

Acesse: `http://localhost:5000/swagger`

### **Testes**

```bash
# Executar todos os testes
dotnet test

# Executar testes de um projeto específico
dotnet test tests/Usuarios.Tests/Usuarios.Tests.csproj

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## Troubleshooting

### **Erro: Connection Refused ao RDS**

Verifique os Security Groups:

```bash
# Security group do EKS deve ter acesso à porta 5432 do RDS
aws ec2 authorize-security-group-ingress \
  --group-id sg-rds-xxx \
  --protocol tcp \
  --port 5432 \
  --source-group sg-eks-xxx \
  --region us-east-1
```

### **Erro: Secrets vazios no Kubernetes**

Recrie os secrets:

```bash
kubectl delete secret users-secret -n users
kubectl create secret generic users-secret \
  --from-literal=ConnectionStrings__PostgreSql="..." \
  --from-literal=ConnectionStrings__DefaultConnection="..." \
  --from-literal=Jwt__Key="..." \
  --namespace=users
```

### **Erro: ICU Package Missing (Alpine)**

O Dockerfile já inclui a instalação do `icu-libs`. Se ainda encontrar o erro:

```bash
# Verificar se o pacote está instalado no container
kubectl exec -it deployment/users-api -n users -- apk info | grep icu
```

### **Token AWS Expirado (AWS Academy)**

Atualize as credenciais:

1. Abra o AWS Academy Lab
2. Clique em "AWS Details"
3. Copie as novas credenciais
4. Atualize `~/.aws/credentials`
5. Atualize os GitHub Secrets

```bash
# Reconectar ao cluster
aws eks update-kubeconfig --region us-east-1 --name users-cluster
```

## Monitoramento com Prometheus e Grafana

O projeto inclui stack completa de monitoramento com Prometheus e Grafana, deployados automaticamente no EKS.

### **Acessar os Serviços de Monitoramento**

```bash
# Obter URLs dos serviços
kubectl get svc -n users

# URL do Prometheus
PROMETHEUS_URL=$(kubectl get svc prometheus -n users -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "Prometheus: http://$PROMETHEUS_URL:9090"

# URL do Grafana
GRAFANA_URL=$(kubectl get svc grafana -n users -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "Grafana: http://$GRAFANA_URL:3000"
echo "Login: admin / admin"
```

### **Prometheus (Coleta de Métricas)**

**Acesso**: `http://<PROMETHEUS_URL>:9090`

**Funcionalidades:**
- `/targets` - Verificar status dos targets (pods da API)
- `/graph` - Executar queries PromQL
- `/metrics` - Métricas do próprio Prometheus

**Queries PromQL Úteis:**

```promql
# Taxa de requisições HTTP
sum(rate(http_requests_received_total{job="usuarios-api"}[5m]))

# Latência p95
histogram_quantile(0.95, sum(rate(http_request_duration_seconds_bucket{job="usuarios-api"}[5m])) by (le))

# Requisições por status code
sum by (code) (rate(http_requests_received_total{job="usuarios-api"}[5m]))

# Uso de memória (MB)
process_working_set_bytes{job="usuarios-api"} / 1024 / 1024

# CPU usage (%)
rate(process_cpu_seconds_total{job="usuarios-api"}[5m]) * 100

# Pods ativos
count(up{job="usuarios-api"} == 1)
```

### **Grafana (Dashboards Visuais)**

**Acesso**: `http://<GRAFANA_URL>:3000`

**Credenciais Padrão:**
- **Usuário**: `admin`
- **Senha**: `admin` (será solicitado trocar no primeiro acesso)

**Dashboard Pré-configurado**: "Usuarios API - AWS EKS"

**Painéis Disponíveis:**
1. 📊 **Taxa de Requisições HTTP** - Requisições por segundo em tempo real
2. ⏱️ **Latência HTTP (p95)** - Tempo de resposta percentil 95
3. 🔢 **Requisições por Status Code** - Distribuição de códigos HTTP (200, 404, 500, etc)
4. 🟢 **Pods Ativos** - Quantidade de pods rodando
5. 💾 **Uso de Memória por Pod** - Consumo de memória em MB
6. ⚡ **CPU Usage por Pod** - Uso de CPU em porcentagem
7. 🔄 **Requisições em Progresso** - Requisições sendo processadas no momento
8. 🧵 **Threads Ativas** - Número de threads por pod
9. ❌ **Taxa de Erros (4xx/5xx)** - Erros do cliente e servidor
10. 🔀 **Requisições por Método HTTP** - Gráfico pizza (GET, POST, PUT, DELETE)
11. 📈 **Latência p50, p95, p99** - Comparação de percentis de latência

### **Gerar Tráfego para Visualizar Métricas**

```bash
# Obter URL da API
API_URL=$(kubectl get svc users-service -n users -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')

# Gerar requisições de teste
for i in {1..50}; do
  curl -s http://$API_URL/health > /dev/null
  curl -s http://$API_URL/swagger > /dev/null
  echo "✓ Request $i"
  sleep 0.5
done

echo "✅ Aguarde 30 segundos e recarregue o Grafana"
```

### **Métricas Expostas pela API**

A aplicação .NET expõe métricas via `/metrics` usando `prometheus-net.AspNetCore`:

```bash
# Ver métricas diretamente da API
curl http://$API_URL/metrics
```

**Principais métricas:**
- `http_requests_received_total` - Total de requisições HTTP
- `http_request_duration_seconds` - Duração das requisições (histogram)
- `http_requests_in_progress` - Requisições em andamento
- `process_working_set_bytes` - Memória working set
- `process_cpu_seconds_total` - Tempo total de CPU
- `process_num_threads` - Número de threads
- `dotnet_collection_count_total` - Contagem de garbage collections

### **Logs do Kubernetes**

```bash
# Logs de todos os pods da API
kubectl logs -n users -l app=users-api --tail=100 -f

# Logs de um pod específico
kubectl logs -n users POD_NAME --tail=100 -f

# Logs anteriores (se o pod reiniciou)
kubectl logs -n users POD_NAME --previous

# Logs do Prometheus
kubectl logs -n users -l app=prometheus --tail=50

# Logs do Grafana
kubectl logs -n users -l app=grafana --tail=50
```

### **CloudWatch Logs**

```bash
# Via AWS CLI
aws logs tail /aws/eks/users-cluster/cluster --follow --region us-east-1
```

### **Port-Forward (Acesso Local sem LoadBalancer)**

Se preferir acessar localmente sem expor LoadBalancers:

```bash
# Prometheus
kubectl port-forward -n users svc/prometheus 9090:9090
# Acesse: http://localhost:9090

# Grafana
kubectl port-forward -n users svc/grafana 3000:3000
# Acesse: http://localhost:3000

# API
kubectl port-forward -n users svc/users-service 8080:80
# Acesse: http://localhost:8080/swagger
```

### **Alertas e Notificações**

Para produção, configure alertas no Prometheus:

```yaml
# Exemplo de regra de alerta
groups:
  - name: usuarios-api
    rules:
      - alert: HighErrorRate
        expr: sum(rate(http_requests_received_total{job="usuarios-api",code=~"5.."}[5m])) > 0.05
        for: 5m
        annotations:
          summary: "Taxa alta de erros 5xx"
      
      - alert: HighLatency
        expr: histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m])) > 1
        for: 5m
        annotations:
          summary: "Latência p95 acima de 1 segundo"
```

## Custos Estimados AWS

Para ambiente de testes (AWS Academy ou Free Tier):

| Recurso            | Tipo         | Custo Mensal (aprox.) |
| ------------------ | ------------ | --------------------- |
| EKS Cluster        | -            | $73.00                |
| EC2 (Worker Nodes) | t3.medium x2 | $60.00                |
| RDS PostgreSQL     | db.t3.micro  | $15.00                |
| LoadBalancer (API) | NLB          | $20.00                |
| LoadBalancer (Prometheus) | NLB   | $20.00                |
| LoadBalancer (Grafana)    | NLB   | $20.00                |
| ECR Storage        | <1GB         | $0.10                 |
| **Total**    |              | **~$208/mês**  |

> ⚠️ **AWS Academy Labs**: Os recursos são reiniciados a cada sessão. Sempre atualize as credenciais.

## Limpeza de Recursos

Para evitar custos desnecessários:

```bash
# Deletar LoadBalancer
kubectl delete service users-service -n users

# Deletar deployment
kubectl delete deployment users-api -n users

# Deletar cluster EKS
aws eks delete-cluster --name users-cluster --region us-east-1

# Deletar RDS
aws rds delete-db-instance \
  --db-instance-identifier users-db \
  --skip-final-snapshot \
  --region us-east-1

# Deletar ECR repository
aws ecr delete-repository \
  --repository-name users-api \
  --force \
  --region us-east-1
```

## Referências

### **Documentação Oficial**

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Kubernetes Documentation](https://kubernetes.io/docs/)

### **AWS Services**

- [Amazon EKS Documentation](https://docs.aws.amazon.com/eks/)
- [Amazon RDS for PostgreSQL](https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/CHAP_PostgreSQL.html)
- [Amazon ECR Documentation](https://docs.aws.amazon.com/ecr/)
- [AWS CLI Command Reference](https://docs.aws.amazon.com/cli/latest/)

### **Best Practices**

- [12 Factor App](https://12factor.net/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [Kubernetes Best Practices](https://kubernetes.io/docs/concepts/configuration/overview/)
- [DDD (Domain-Driven Design)](https://martinfowler.com/tags/domain%20driven%20design.html)

## Estrutura de Arquivos Kubernetes

```
k8s/
├── configmap.yaml           → Configurações públicas
├── secret.yaml.template     → Template de secrets (com envsubst)
├── deployment.yaml          → Deployment com 2 réplicas
└── service.yaml            → LoadBalancer service
```

## CI/CD Pipeline

O projeto utiliza GitHub Actions para automação completa:

**Workflow: `.github/workflows/deploy_aws.yml`**

1. **Build & Test**: Compila e executa testes unitários
2. **Docker Build**: Cria imagem otimizada com Alpine
3. **ECR Push**: Envia imagem para Amazon ECR
4. **Deploy**: Aplica manifests Kubernetes no EKS
5. **Validation**: Verifica status do deployment

**Trigger**: Push ou Pull Request na branch `main`

## Como rodar o projeto

#### **Desenvolvimento Local**

#### **Pré-requisitos**

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [pgAdmin](https://www.pgadmin.org/download/) ou [DBeaver](https://dbeaver.io/download/) (opcional, para gerenciamento visual do PostgreSQL)

#### **Banco de Dados PostgreSQL com Docker**

```bash
docker run --name postgres-local \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=users_db \
  -p 5432:5432 \
  -d postgres:15-alpine
```

#### **Executar Migrations**

```bash
dotnet ef migrations add "NomeDaMigration" --project src/Usuarios.Infrastructure --startup-project src/Usuarios.API
dotnet ef database update --project src/Usuarios.Infrastructure --startup-project src/Usuarios.API
```

## Contribuição

Se deseja contribuir, fique à vontade para abrir issues ou enviar pull requests. Toda colaboração é bem-vinda!

## Licença

Este projeto está licenciado sob a **MIT License**.
