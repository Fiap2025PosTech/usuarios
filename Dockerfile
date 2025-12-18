# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copia apenas os .csproj e restaura dependências (melhor cache)
COPY ["src/Usuarios.API/Usuarios.API.csproj", "Usuarios.API/"]
COPY ["src/Usuarios.Application/Usuarios.Application.csproj", "Usuarios.Application/"]
COPY ["src/Usuarios.Domain/Usuarios.Domain.csproj", "Usuarios.Domain/"]
COPY ["src/Usuarios.Infrastructure/Usuarios.Infrastructure.csproj", "Usuarios.Infrastructure/"]

RUN dotnet restore "Usuarios.API/Usuarios.API.csproj"

# Copia o código fonte
COPY src/ .

# Build e publish com otimizações
WORKDIR /src/Usuarios.API
RUN dotnet publish "Usuarios.API.csproj" -c Release -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false \
    /p:PublishSingleFile=false

# Stage 2: Runtime (Alpine - muito mais leve)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

# Instala dependências ICU para globalização
RUN apk add --no-cache icu-libs

# Cria usuário não-root para segurança
RUN addgroup -g 1000 appuser && adduser -u 1000 -G appuser -s /bin/sh -D appuser

WORKDIR /app

# Copia os arquivos publicados
COPY --from=build --chown=appuser:appuser /app/publish .

# Muda para usuário não-root
USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "Usuarios.API.dll"]