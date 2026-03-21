# Payment API

API REST responsável pelo gerenciamento de **pagamentos** em uma arquitetura baseada em **microserviços**, preparada para execução em **containers Docker** e deploy em **Kubernetes (K8s)**.

Este projeto demonstra conceitos fundamentais de **Cloud-Native Applications**, incluindo:

* Containerização da aplicação
* Deploy declarativo em Kubernetes
* Gerenciamento de configuração com ConfigMap
* Armazenamento seguro de credenciais com Secrets
* Orquestração de containers com Kubernetes

O repositório faz parte de um conjunto de serviços que demonstram **microserviços executando em um cluster Kubernetes**.

---

# Arquitetura da Aplicação

A aplicação segue o padrão comum de workloads executados em Kubernetes.

```
      Client
        │
        ▼
   Kubernetes Service
        │
        ▼
     Deployment
        │
        ▼
       Pods
   (Payment API)
        │
        ├── ConfigMap
        └── Secret
```

Componentes utilizados:

| Componente | Função                               |
| ---------- | ------------------------------------ |
| Deployment | Gerenciamento e atualização dos pods |
| Service    | Comunicação entre serviços           |
| ConfigMap  | Configurações da aplicação           |
| Secret     | Credenciais e dados sensíveis        |

---

# Tecnologias Utilizadas

Backend

* .NET

Infraestrutura

* Docker
* Kubernetes
* YAML Manifests
* kubectl

---

# Estrutura do Repositório

```
k8s-payment-api
│
├── src/
│   └── PaymentAPI
│
├── k8s/
│   ├── configmap.yaml
│   ├── secret.yaml
│   ├── deployment.yaml
│   └── service.yaml
│
├── Dockerfile
├── .dockerignore
└── README.md
```

---

# Pré-requisitos

Para executar o projeto é necessário possuir instalado:

* .NET SDK
* Docker
* Kubernetes Cluster
* kubectl

Clusters locais recomendados:

* Minikube
* Kind
* Docker Desktop Kubernetes

---

# Executando a Aplicação Localmente

Clone o repositório:

```bash
git clone https://github.com/techGabrielBr/k8s-payment-api.git
cd k8s-payment-api
```

Executar a aplicação:

```bash
dotnet restore
dotnet build
dotnet run --project src/PaymentAPI
```

A API ficará disponível em:

```
http://localhost:5000
```

---

# Containerização com Docker

Construir a imagem da aplicação:

```bash
docker build -t payment-api .
```

Executar o container:

```bash
docker run -p 5000:5000 payment-api
```

A API ficará acessível em:

```
http://localhost:5000
```

---

# Deploy no Kubernetes

Os manifests Kubernetes estão localizados na pasta:

```
k8s/
```

Aplicar todos os recursos no cluster:

```bash
kubectl apply -f k8s/
```

---

# Verificando os Recursos

Listar recursos criados:

```bash
kubectl get all
```

Ver pods em execução:

```bash
kubectl get pods
```

Ver services:

```bash
kubectl get svc
```

Ver logs de um pod:

```bash
kubectl logs <nome-do-pod>
```

---

# Descrição dos Manifests Kubernetes

## Namespace

Arquivo:

```
k8s/namespace.yaml
```

Cria um namespace dedicado para a aplicação dentro do cluster Kubernetes.

Benefícios:

* isolamento entre aplicações
* organização do cluster
* separação entre ambientes

---

## ConfigMap

Arquivo:

```
k8s/configmap.yaml
```

Responsável por armazenar **configurações não sensíveis** da aplicação.

Exemplos de uso:

* parâmetros da aplicação
* URLs de serviços externos
* configurações de ambiente

Essas configurações são injetadas nos containers como variáveis de ambiente.

---

## Secret

Arquivo:

```
k8s/secret.yaml
```

Armazena **dados sensíveis** utilizados pela aplicação.

Exemplos:

* connection strings
* tokens
* credenciais

Secrets são armazenados em **base64** e podem ser consumidos como:

* variáveis de ambiente
* arquivos montados em volume

---

## Deployment

Arquivo:

```
k8s/deployment.yaml
```

Define o Deployment da aplicação.

Responsabilidades:

* criar pods
* manter pods ativos
* recriar pods em caso de falha
* permitir atualizações da aplicação

Configurações típicas:

* imagem Docker
* número de réplicas
* portas do container
* variáveis de ambiente
* integração com ConfigMap e Secret

---

## Service

Arquivo:

```
k8s/service.yaml
```

Cria um Service para permitir comunicação entre os pods da aplicação e outros serviços dentro do cluster.

Funções:

* endpoint estável
* balanceamento de carga entre pods
* abstração sobre IPs dinâmicos dos pods

---

# Objetivo do Projeto

Este projeto foi criado para demonstrar:

* deploy de APIs containerizadas em Kubernetes
* arquitetura baseada em microserviços
* boas práticas de infraestrutura cloud-native
* separação entre aplicação e infraestrutura

Ele também pode ser utilizado como base para estudo de **deploy de aplicações backend em clusters Kubernetes**.

---
