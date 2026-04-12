# Invoice Processor (Event-Driven + AI Powered)

An enterprise-grade **event-driven document processing system** built with **.NET, Azure Services, and OpenAI**.
This system processes uploaded invoices asynchronously, extracts structured data using AI, and stores results for downstream use.

---

# Overview

This project demonstrates:

* Clean Architecture (Domain, Application, Infrastructure, API)
* Event-driven design using Azure Service Bus
* Asynchronous processing with Azure Functions
* AI-powered document extraction using OpenAI
* Production-ready patterns (idempotency, retries, DLQ, validation)

---

# Architecture

```
Client
  ↓
Web API (.NET)
  ↓
Azure Blob Storage
  ↓
Azure Service Bus Queue
  ↓
Azure Function (Processor)
  ↓
PostgreSQL Database
```

---

# Tech Stack

### Backend

* .NET 8 (Web API + Azure Functions)
* Entity Framework Core
* PostgreSQL

### Azure Services

* Azure Blob Storage
* Azure Service Bus
* Azure Functions (Isolated Worker)

### AI

* OpenAI (GPT-4.1-mini)

---

# Features

## File Upload API

* Secure file validation (type, size, extension)
* Clean Architecture with MediatR
* Correlation ID tracking

## Event-Driven Processing

* Message-based workflow using Service Bus
* Asynchronous document processing

## AI Extraction

* Extracts:

  * Vendor
  * Amount
  * Invoice Date
* Converts unstructured text → structured JSON

## Resilience & Reliability

* Retry policies (Polly)
* Dead-letter queue handling
* Idempotent processing (no duplicate work)

## Observability

* Structured logging
* Correlation ID propagation across services

---

# Project Structure

```
/src
  /Domain
  /Application
  /Infrastructure
  /API
  /Functions
  /Contracts
```

---

# Processing Flow

1. User uploads invoice via API
2. File stored in Blob Storage
3. Message sent to Service Bus
4. Azure Function processes message:

   * Reads file from Blob
   * Extracts data using OpenAI
   * Saves structured data to DB
5. Status updated (Completed / Failed)

---

# Local Development Setup

## Prerequisites

* .NET 8 SDK
* Docker (for PostgreSQL)
* Azure Functions Core Tools
* Azurite (local Blob Storage)

---

## Run Services

### Start Azurite

```
azurite
```

### Run API

```
cd API
dotnet run
```

### Run Functions

```
cd Functions
func start
```

---

# Configuration

## API & Functions (`appsettings.json` / `local.settings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=docs;Username=postgres;Password=postgres"
  },
  "Blob": {
    "ConnectionString": "UseDevelopmentStorage=true",
    "ContainerName": "uploads"
  },
  "ServiceBus": {
    "ConnectionString": "<your-service-bus-connection>",
    "QueueName": "documents"
  },
  "OpenAI": {
    "ApiKey": "<your-openai-key>"
  }
}
```

---

# Security Considerations

* File validation (size, type, extension)
* Input sanitization
* No hardcoded secrets (use environment variables)

---

# Key Engineering Concepts

* Clean Architecture (separation of concerns)
* Dependency Injection
* Idempotency in distributed systems
* Correlation ID tracing
* Retry & dead-letter strategies
* AI integration in backend workflows

---

# Future Improvements

* Dashboard (Blazor/Angular)
* Manual DLQ reprocessing UI
* Role-based authentication (JWT / Azure AD B2C)
* Advanced AI extraction with schema validation
* Multi-tenant support

---

# Portfolio Value

This project demonstrates:

* Real-world cloud architecture
* Distributed systems thinking
* AI integration in backend services
* Production-ready engineering practices

---

# Contributing

Pull requests are welcome. For major changes, please open an issue first.

---

# License

MIT License
