<div align="center">

# 🏋️ FunctionFit

### Gym Management REST API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/en-us/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=flat-square)](https://learn.microsoft.com/en-us/ef/core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)

*A comprehensive gym management system with subscription plans, class scheduling, enrollment, and integrated payment processing.*

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running the Application](#running-the-application)
- [API Reference](#-api-reference)
- [Database](#-database)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)

---

## 🧭 Overview

**FunctionFit** is a RESTful API built with ASP.NET Core (.NET 8) for managing gym operations. It covers the full lifecycle of gym management: member registration, subscription plans, class scheduling, enrollment, and payment tracking through Mercado Pago.

The project follows **Clean Architecture** principles, separating concerns across Domain, Application, Infrastructure, and Presentation layers.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔐 **Authentication & Authorization** | JWT-based login, registration, password reset via email |
| 👥 **Role-Based Access Control** | Three roles: Socio (Member), Administrador, SuperAdministrador |
| 📦 **Subscription Plans** | Basic, Premium, and Elite tiers with different class enrollment limits |
| 🗓️ **Class Scheduling** | Create and manage gym classes (Yoga, CrossFit, Spinning, Pilates, etc.) with capacity control |
| 📝 **Enrollment** | Members enroll in classes; enrollment quota depends on their subscription plan |
| 💳 **Payments** | Integrated with Mercado Pago for payment processing and webhook handling |
| 📧 **Email Notifications** | Password reset emails via Mailtrap SMTP |
| 📊 **Attendance History** | Track class participation history per user |
| 📖 **API Documentation** | Swagger UI available at runtime |

---

## 🏗️ Architecture

FunctionFit follows **Clean Architecture** with four well-defined layers:

```
┌─────────────────────────────────────────┐
│          PresentationAPI-TP             │  ← HTTP Controllers, Swagger, Auth Middleware
├─────────────────────────────────────────┤
│              Application                │  ← Business Logic, Use Cases, Service Interfaces
├─────────────────────────────────────────┤
│              Contracts                  │  ← DTOs, Request/Response Models
├─────────────────────────────────────────┤
│               Domain                    │  ← Entities, Domain Models
├─────────────────────────────────────────┤
│            Infrastructure               │  ← EF Core, Repositories, External Services
└─────────────────────────────────────────┘
```

Each layer depends only on the layers below it. The `Contracts` layer is shared across boundaries as a DTO transport layer.

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| **Framework** | .NET 8.0, ASP.NET Core Web API |
| **ORM** | Entity Framework Core 8.0 |
| **Database** | SQL Server / SQLite |
| **Authentication** | JWT Bearer Tokens, ASP.NET Identity Password Hasher |
| **Payment Gateway** | Mercado Pago SDK 2.0 |
| **Email** | Mailtrap SMTP |
| **API Docs** | Swashbuckle / Swagger UI 6.6.2 |
| **Resilience** | Polly (retry policies for DB connections) |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQLite (configured by default)
- A [Mercado Pago](https://www.mercadopago.com.ar/developers/) developer account (for payment features)
- A [Mailtrap](https://mailtrap.io/) account (for email features)

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/your-org/TP-ProgramacionIV-UTN.git
   cd TP-ProgramacionIV-UTN
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Apply database migrations**

   ```bash
   dotnet ef database update --project Infrastructure --startup-project PresentationAPI-TP
   ```

   > The database is seeded automatically on first run with roles, default users, plans, and sample gym classes.

### Configuration

Copy or edit `PresentationAPI-TP/appsettings.json` and fill in your own values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "gym.db",
    "SqlServerConnection": "Server=YOUR_SERVER;Database=FunctionFit;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "https://your-domain.com",
    "Audience": "https://your-domain.com"
  },
  "MercadoPago": {
    "AccessToken": "YOUR_MP_ACCESS_TOKEN",
    "PublicKey": "YOUR_MP_PUBLIC_KEY"
  },
  "Frontend": {
    "BaseUrl": "http://localhost:4200"
  },
  "MailtrapSettings": {
    "Host": "sandbox.smtp.mailtrap.io",
    "Port": 587,
    "Username": "YOUR_MAILTRAP_USER",
    "Password": "YOUR_MAILTRAP_PASSWORD",
    "FromEmail": "noreply@functionfit.com"
  }
}
```

> ⚠️ **Never commit real secrets to source control.** Use [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables in production.

### Running the Application

```bash
dotnet run --project PresentationAPI-TP
```

The API will be available at `https://localhost:7xxx` (port shown in console output).  
Swagger UI: `https://localhost:7xxx/swagger`

---

## 📡 API Reference

### Authentication

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `POST` | `/api/auth/register` | Register a new member | ❌ |
| `POST` | `/api/auth/login` | Login and receive JWT token | ❌ |
| `POST` | `/api/auth/forgot-password` | Request password reset email | ❌ |
| `POST` | `/api/auth/reset-password` | Reset password with token | ❌ |

### Users

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `GET` | `/api/user/me` | Get current user profile | ✅ |
| `PUT` | `/api/user/{id}` | Update user profile | ✅ |
| `GET` | `/api/admin/users` | List all users | ✅ Admin |
| `POST` | `/api/admin/users` | Create user (admin) | ✅ Admin |
| `DELETE` | `/api/admin/users/{id}` | Delete user | ✅ Admin |

### Plans & Subscriptions

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `GET` | `/api/plan` | List all plans | ✅ |
| `POST` | `/api/plan` | Create a plan | ✅ Admin |
| `PUT` | `/api/plan/{id}` | Update a plan | ✅ Admin |
| `DELETE` | `/api/plan/{id}` | Delete a plan | ✅ Admin |

### Gym Classes & Enrollment

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `GET` | `/api/gymclass` | List all gym classes | ✅ |
| `POST` | `/api/gymclass` | Create a class | ✅ Admin |
| `PUT` | `/api/gymclass/{id}` | Update a class | ✅ Admin |
| `DELETE` | `/api/gymclass/{id}` | Delete a class | ✅ Admin |
| `POST` | `/api/enrollment` | Enroll in a class | ✅ |
| `DELETE` | `/api/enrollment/{id}` | Cancel enrollment | ✅ |

### Payments

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `GET` | `/api/payment` | List payments for current user | ✅ |
| `POST` | `/api/payment` | Create a payment / MP preference | ✅ |
| `POST` | `/api/payment/webhook` | Mercado Pago webhook receiver | ❌ |

### History

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `GET` | `/api/historical` | Get attendance history | ✅ |

> 📖 Full interactive documentation available via Swagger UI when the app is running.

---

## 🗄️ Database

### Entity Relationship Overview

```
User ──────< Subscription >────── Plan
 │
 ├──────< Payment
 │
 ├──────< Historical
 │
 └──────< UserGymClass >────── GymClass
```

### Default Seed Data

The database is seeded with the following data on first run:

**Roles**

| Role | Description |
|---|---|
| Socio | Regular gym member |
| Administrador | Gym staff / manager |
| SuperAdministrador | Full system access |

**Subscription Plans**

| Plan | Price | Max Classes |
|---|---|---|
| Basic | $25 / month | 5 classes |
| Premium | $45 / month | 10 classes |
| Elite | $70 / month | 15 classes |

**Default Users** *(password: `1234` — change immediately in production)*

| Email | Role |
|---|---|
| `client@gym.com` | Socio |
| `admin@gym.com` | Administrador |
| `superadmin@gym.com` | SuperAdministrador |

---

## 📁 Project Structure

```
TP-ProgramacionIV-UTN/
├── PresentationAPI-TP/          # Web API entry point
│   ├── Controllers/             # HTTP controllers (Auth, User, GymClass, etc.)
│   ├── Program.cs               # App bootstrap and DI configuration
│   └── appsettings.json         # App configuration
│
├── Application/                 # Business logic layer
│   ├── Services/                # Service implementations
│   ├── Interfaces/              # Service contracts
│   └── ServiceCollectionExtension.cs
│
├── Contracts/                   # Shared DTOs
│   ├── User/
│   ├── GymClass/
│   ├── Payment/
│   ├── Plan/
│   ├── Enrollment/
│   └── Historical/
│
├── Domain/                      # Core domain entities
│   ├── User.cs
│   ├── GymClass.cs
│   ├── Plan.cs
│   ├── Subscription.cs
│   ├── Payment.cs
│   ├── Historical.cs
│   ├── Role.cs
│   └── BaseEntity.cs
│
├── Infrastructure/              # Data access & external services
│   ├── Persistence/
│   │   ├── GymDbContext.cs      # EF Core DbContext
│   │   └── Migrations/
│   ├── Repositories/            # Repository implementations
│   ├── Abstraction/             # Repository interfaces
│   └── ExternalServices/        # JWT, Email, MercadoPago, Password
│
└── PresentationAPI-TP.sln
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "feat: add your feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

Please follow [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.

---

<div align="center">

Made with ❤️ · Universidad Tecnológica Nacional — Programación IV

</div>