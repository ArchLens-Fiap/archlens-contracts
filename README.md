# 📦 ArchLens - Contracts

> **Contratos de Eventos para Comunicação entre Microsserviços**
> Hackathon FIAP - Fase 5 | Pós-Tech Software Architecture + IA para Devs
>
> **Autor:** Rafael Henrique Barbosa Pereira (RM366243)

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MassTransit](https://img.shields.io/badge/MassTransit-8.x-512BD4)](https://masstransit.io/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.x-FF6600?logo=rabbitmq)](https://www.rabbitmq.com/)

---

## 📋 Descrição

Biblioteca compartilhada contendo os **contratos de eventos** utilizados na comunicação assíncrona entre os microsserviços do ArchLens via **Saga Orquestrada** com RabbitMQ e MassTransit. Todos os serviços .NET referenciam este projeto para garantir consistência nos payloads de mensagens.

---

## 🏗️ Arquitetura

```mermaid
graph TB
    subgraph "Microsserviços"
        UP[Upload Service]
        ORCH[Orchestrator Service]
        REP[Report Service]
        AUTH[Auth Service]
        NOTIF[Notification Service]
        AI[AI Service]
    end

    subgraph "Shared Library"
        CONTRACTS[ArchLens.Contracts]
    end

    subgraph "Message Broker"
        RMQ[RabbitMQ + MassTransit]
    end

    UP --> CONTRACTS
    ORCH --> CONTRACTS
    REP --> CONTRACTS
    AUTH --> CONTRACTS
    NOTIF --> CONTRACTS

    CONTRACTS --> RMQ
```

---

## 🔄 Fluxo Completo da Saga (Happy Path)

```mermaid
sequenceDiagram
    participant U as Usuário
    participant UP as Upload Service
    participant ORCH as Orchestrator
    participant AI as AI Service
    participant REP as Report Service
    participant NOTIF as Notification

    rect rgb(200, 230, 200)
        Note over U,NOTIF: 1. Upload do Diagrama
        U->>UP: POST /api/upload
        UP->>ORCH: DiagramUploadedEvent
    end

    rect rgb(200, 220, 240)
        Note over U,NOTIF: 2. Início do Processamento
        ORCH->>ORCH: Cria Analysis
        ORCH->>NOTIF: StatusChangedEvent (Processing)
        ORCH->>AI: ProcessingStartedEvent
    end

    rect rgb(240, 220, 200)
        Note over U,NOTIF: 3. Análise com IA
        AI->>ORCH: AnalysisCompletedEvent
        ORCH->>NOTIF: StatusChangedEvent (Analyzed)
    end

    rect rgb(220, 240, 220)
        Note over U,NOTIF: 4. Geração de Relatório
        ORCH->>REP: GenerateReportCommand
        REP->>ORCH: ReportGeneratedEvent
        ORCH->>NOTIF: StatusChangedEvent (Completed)
    end

    rect rgb(200, 200, 240)
        Note over U,NOTIF: 5. Notificação Final
        NOTIF->>U: Push Notification (SignalR)
    end
```

---

## 🔥 Fluxo de Compensação (Falhas)

```mermaid
sequenceDiagram
    participant ORCH as Orchestrator
    participant AI as AI Service
    participant REP as Report Service
    participant NOTIF as Notification

    rect rgb(255, 200, 200)
        Note over ORCH,NOTIF: Cenário: Falha na Análise IA
        AI->>ORCH: AnalysisFailedEvent
        ORCH->>NOTIF: StatusChangedEvent (Failed)
        Note over ORCH: Compensação: Retry ou Status → Failed
    end

    rect rgb(255, 220, 200)
        Note over ORCH,NOTIF: Cenário: Falha na Geração do Relatório
        REP->>ORCH: ReportFailedEvent
        ORCH->>NOTIF: StatusChangedEvent (ReportFailed)
        Note over ORCH: Compensação: Retry ou Status → Failed
    end
```

---

## 📡 Mapa de Eventos

```mermaid
graph LR
    subgraph "Upload Service"
        A[DiagramUploadedEvent]
    end

    subgraph "Orchestrator Service"
        B[ProcessingStartedEvent]
        C[StatusChangedEvent]
        D[GenerateReportCommand]
    end

    subgraph "AI Service"
        E[AnalysisCompletedEvent]
        F[AnalysisFailedEvent]
    end

    subgraph "Report Service"
        G[ReportGeneratedEvent]
        H[ReportFailedEvent]
    end

    subgraph "Auth Service"
        I[UserAccountDeletedEvent]
    end

    A --> B
    B --> E
    B --> F
    E --> D
    D --> G
    D --> H
    C --> NOTIF[Notification Service]
    I --> ORCH2[Orchestrator / Upload]
```

---

## 📊 Tabela de Eventos

| Evento | Publicado Por | Consumido Por | Descrição |
|--------|---------------|---------------|-----------|
| `DiagramUploadedEvent` | Upload Service | Orchestrator | Diagrama enviado e armazenado |
| `ProcessingStartedEvent` | Orchestrator | AI Service | Análise iniciada, diagrama pronto para IA |
| `AnalysisCompletedEvent` | AI Service | Orchestrator | Análise IA concluída com sucesso |
| `AnalysisFailedEvent` | AI Service | Orchestrator | Falha na análise IA (compensação) |
| `StatusChangedEvent` | Orchestrator | Notification | Status da análise alterado |
| `GenerateReportCommand` | Orchestrator | Report Service | Comando para gerar relatório |
| `ReportGeneratedEvent` | Report Service | Orchestrator | Relatório gerado com sucesso |
| `ReportFailedEvent` | Report Service | Orchestrator | Falha na geração do relatório (compensação) |
| `UserAccountDeletedEvent` | Auth Service | Orchestrator, Upload | Conta de usuário deletada (LGPD) |

---

## 📦 Estrutura dos Eventos (Payloads)

### DiagramUploadedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `DiagramId` | `Guid` | ID do diagrama |
| `UserId` | `Guid` | ID do usuário |
| `FileName` | `string` | Nome do arquivo |
| `StoragePath` | `string` | Caminho no MinIO |
| `ContentType` | `string` | Tipo MIME do arquivo |
| `UploadedAt` | `DateTime` | Data/hora do upload |
| `CorrelationId` | `Guid` | ID de correlação |

### ProcessingStartedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `DiagramId` | `Guid` | ID do diagrama |
| `UserId` | `Guid` | ID do usuário |
| `StoragePath` | `string` | Caminho do diagrama |
| `StartedAt` | `DateTime` | Data/hora do início |
| `CorrelationId` | `Guid` | ID de correlação |

### AnalysisCompletedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `DiagramId` | `Guid` | ID do diagrama |
| `Result` | `string` | Resultado da análise (JSON) |
| `Provider` | `string` | Provider IA utilizado |
| `CompletedAt` | `DateTime` | Data/hora da conclusão |
| `CorrelationId` | `Guid` | ID de correlação |

### AnalysisFailedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `DiagramId` | `Guid` | ID do diagrama |
| `Reason` | `string` | Motivo da falha |
| `FailedAt` | `DateTime` | Data/hora da falha |
| `CorrelationId` | `Guid` | ID de correlação |

### StatusChangedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `UserId` | `Guid` | ID do usuário |
| `OldStatus` | `string` | Status anterior |
| `NewStatus` | `string` | Novo status |
| `ChangedAt` | `DateTime` | Data/hora da mudança |
| `CorrelationId` | `Guid` | ID de correlação |

### GenerateReportCommand

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `DiagramId` | `Guid` | ID do diagrama |
| `AnalysisResult` | `string` | Resultado da análise (JSON) |
| `UserId` | `Guid` | ID do usuário |
| `RequestedAt` | `DateTime` | Data/hora da solicitação |
| `CorrelationId` | `Guid` | ID de correlação |

### ReportGeneratedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `ReportId` | `Guid` | ID do relatório |
| `AnalysisId` | `Guid` | ID da análise |
| `StoragePath` | `string` | Caminho do relatório |
| `GeneratedAt` | `DateTime` | Data/hora da geração |
| `CorrelationId` | `Guid` | ID de correlação |

### ReportFailedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `AnalysisId` | `Guid` | ID da análise |
| `Reason` | `string` | Motivo da falha |
| `FailedAt` | `DateTime` | Data/hora da falha |
| `CorrelationId` | `Guid` | ID de correlação |

### UserAccountDeletedEvent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `UserId` | `Guid` | ID do usuário deletado |
| `Email` | `string` | Email do usuário |
| `DeletedAt` | `DateTime` | Data/hora da exclusão |
| `CorrelationId` | `Guid` | ID de correlação |

---

## 📁 Estrutura do Projeto

```
archlens-contracts/
├── src/
│   └── ArchLens.Contracts/
│       ├── Events/
│       │   ├── DiagramUploadedEvent.cs
│       │   ├── ProcessingStartedEvent.cs
│       │   ├── AnalysisCompletedEvent.cs
│       │   ├── AnalysisFailedEvent.cs
│       │   ├── StatusChangedEvent.cs
│       │   ├── GenerateReportCommand.cs
│       │   ├── ReportGeneratedEvent.cs
│       │   ├── ReportFailedEvent.cs
│       │   └── UserAccountDeletedEvent.cs
│       └── ArchLens.Contracts.csproj
└── ArchLens.Contracts.sln
```

---

## 🚀 Como Usar

### Adicionar Referência

```bash
dotnet add reference ../archlens-contracts/src/ArchLens.Contracts/ArchLens.Contracts.csproj
```

### Publicar Evento

```csharp
using ArchLens.Contracts.Events;

await _publishEndpoint.Publish(new DiagramUploadedEvent
{
    DiagramId = diagram.Id,
    UserId = userId,
    FileName = file.FileName,
    StoragePath = storagePath,
    ContentType = file.ContentType,
    UploadedAt = DateTime.UtcNow,
    CorrelationId = Guid.NewGuid()
});
```

### Consumir Evento

```csharp
using ArchLens.Contracts.Events;

public class DiagramUploadedConsumer : IConsumer<DiagramUploadedEvent>
{
    public async Task Consume(ConsumeContext<DiagramUploadedEvent> context)
    {
        var @event = context.Message;

        var analysis = Analysis.Create(
            @event.DiagramId,
            @event.UserId,
            @event.StoragePath
        );

        await _repository.AddAsync(analysis);
    }
}
```

### Registrar Consumer (MassTransit)

```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<DiagramUploadedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // MassTransit kebab-case endpoints
        cfg.ReceiveEndpoint("orchestrator-diagram-uploaded", e =>
        {
            e.ConfigureConsumer<DiagramUploadedConsumer>(context);
        });
    });
});
```

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| .NET | 9.0 | Framework |
| MassTransit | 8.x | Abstração de mensageria (kebab-case endpoints) |
| RabbitMQ | 3.x | Message Broker |

---

## 🔧 Build

```bash
dotnet build
dotnet pack -o ./nupkg
```

---

FIAP - Pós-Tech Software Architecture + IA para Devs | Fase 5 - Hackathon (12SOAT + 6IADT)
