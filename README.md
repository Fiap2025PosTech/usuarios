# Usuarios

Projeto base para gerenciamento de usuários, seguindo arquitetura em camadas:

- API
- Application
- Domain
- Infrastructure

## Estrutura

```
src/
  Usuarios.API
  Usuarios.Application
  Usuarios.Domain
  Usuarios.Infrastructure
monitoring/
  grafana/
  prometheus.yml
Dockerfile
docker-compose.yml
docker-compose.monitoring.yml
tests/
```

## Como rodar

1. Restaure os pacotes:
   ```bash
   dotnet restore
   ```
2. Compile a solução:
   ```bash
   dotnet build
   ```
3. Execute a API:
   ```bash
   dotnet run --project src/Usuarios.API/Usuarios.API.csproj
   ```

## Observações

- Configurações de monitoramento estão na pasta `monitoring`.
- Utilize Docker Compose para subir o ambiente completo.
