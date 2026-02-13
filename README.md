# Product Management API - Onion Architecture

Modern mimari prensipleriyle geliştirilmiş, ölçeklenebilir ve sürdürülebilir bir ürün yönetim API'si.

## 🏗️ Mimari

Bu proje **Onion Architecture** (Soğan Mimarisi) kullanılarak geliştirilmiştir. Katmanlar merkeze doğru bağımlıdır ve iş mantığı dış dünyadan izole edilmiştir.


**1. Core (Domain Layer)**
- Entities (Product, User)
- Core Interfaces (IRepository, ICacheService)
- İş kuralları ve domain logic

**2. Application Layer**
- DTOs (Data Transfer Objects)
- CQRS Pattern (Commands & Queries)
- Service Interfaces
- İş mantığı soyutlamaları

**3. Infrastructure Layer**
- DbContext (Entity Framework Core)
- Repositories (Concrete implementations)
- Services (ProductService, AuthService, TokenService)
- Caching (Redis)
- External dependencies

**4. API (Presentation Layer)**
- Controllers (RESTful endpoints)
- Middleware (Global exception handler)
- Configuration (Program.cs)
- Swagger/OpenAPI

## 🛠️ Kullanılan Teknolojiler

- **.NET 10.0** - ASP.NET Core Web API
- **C#** - Programlama dili
- **PostgreSQL** - İlişkisel veritabanı
- **Entity Framework Core 9.0** - ORM
- **Redis** - Distributed caching
- **JWT** - Authentication & Authorization
- **BCrypt** - Password hashing
- **Swagger/OpenAPI** - API documentation
- **Serilog** - Structured logging

## � Özellikler

### Mimari Patterns
- ✅ **Onion Architecture** - Katmanlı ve bağımlılık ters çevirme
- ✅ **CQRS Pattern** - Command Query Responsibility Segregation
- ✅ **Repository Pattern** - Veri erişim soyutlaması
- ✅ **Dependency Injection** - Gevşek bağlılık
- ✅ **DTO Pattern** - Veri transfer objeleri

### Teknik Özellikler
- ✅ JWT Authentication - Güvenli kimlik doğrulama
- ✅ Redis Caching - Performans optimizasyonu
- ✅ Global Exception Handling - Merkezi hata yönetimi
- ✅ Async/Await - Asenkron programlama
- ✅ Input Validation - Veri doğrulama
- ✅ Password Hashing - BCrypt ile güvenli şifreleme
- ✅ Swagger UI - İnteraktif API dokümantasyonu

## 📁 Proje Yapısı

```
src/
├── Core/                  # Domain Layer
│   ├── Entities/          # Product, User
│   └── Interfaces/        # IRepository, ICacheService
├── Application/           # Application Layer
│   ├── DTOs/              # Data Transfer Objects
│   ├── Commands/          # CQRS Commands
│   ├── Queries/           # CQRS Queries
│   └── Interfaces/        # Service Interfaces
├── Infrastructure/        # Infrastructure Layer
│   ├── Data/              # DbContext
│   ├── Repositories/      # Repository Implementations
│   ├── Services/          # Business Services
│   └── Caching/           # Redis Cache
└── API/                   # Presentation Layer
    ├── Controllers/       # REST Controllers
    ├── Middleware/        # Exception Handler
    └── Program.cs         # Configuration
```

## 📋 Gereksinimler

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Redis](https://redis.io/download) (opsiyonel, cache için)

## 🚀 Kurulum

### 1. Projeyi Klonlayın

```bash
git clone <repository-url>
cd kayra-task
```

### 2. PostgreSQL Veritabanını Oluşturun

```sql
CREATE DATABASE kayra_task;
```

### 3. Bağlantı Ayarlarını Yapılandırın

`src/API/appsettings.json` dosyasını düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kayra_task;Username=postgres;Password=yourpassword",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "super-secret-jwt-key-minimum-32-characters-long",
    "Issuer": "ProductManagementAPI",
    "Audience": "ProductManagementClient"
  }
}
```

### 4. Paketleri Yükleyin

```bash
dotnet restore
```

### 5. Veritabanı Migration'ını Çalıştırın

```bash
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/API
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

### 6. Redis'i Başlatın (Opsiyonel)

**Docker ile:**
```bash
docker run -d --name redis -p 6379:6379 redis
```

### 7. Uygulamayı Çalıştırın

```bash
cd src/API
dotnet run
```

**Swagger UI:** `http://localhost:5214`

## 🗂️ Veritabanı Şeması

### Products Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | INTEGER | Primary Key, otomatik artan |
| Name | VARCHAR(200) | Ürün adı (zorunlu) |
| Description | VARCHAR(1000) | Ürün açıklaması (opsiyonel) |
| Price | DECIMAL(18,2) | Ürün fiyatı (zorunlu, > 0) |
| Stock | INTEGER | Stok miktarı (zorunlu, >= 0) |
| CreatedDate | TIMESTAMP | Oluşturulma tarihi (otomatik) |
| UpdatedDate | TIMESTAMP | Güncellenme tarihi (opsiyonel) |

### Users Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | INTEGER | Primary Key, otomatik artan |
| Username | VARCHAR(50) | Kullanıcı adı (zorunlu, unique) |
| Email | VARCHAR(100) | Email (zorunlu, unique) |
| PasswordHash | VARCHAR(255) | BCrypt hash (zorunlu) |
| Role | VARCHAR(20) | Kullanıcı rolü (varsayılan: "User") |
| CreatedDate | TIMESTAMP | Oluşturulma tarihi (otomatik) |
| UpdatedDate | TIMESTAMP | Güncellenme tarihi (opsiyonel) |

## 🔧 Geliştirme

### Yeni Migration Oluşturma

```bash
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/API
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

### Build

```bash
dotnet build
```

### Clean

```bash
dotnet clean
```

## 🧪 Test

Swagger UI kullanarak API'yi test edebilirsiniz:

1. Uygulamayı çalıştırın: `dotnet run`
2. Tarayıcıda açın: `http://localhost:5214`
3. Swagger arayüzünden endpoint'leri test edin

**Test Senaryosu:**
1. `/api/auth/register` ile kullanıcı oluşturun
2. `/api/auth/login` ile token alın
3. `/api/products` ile ürün ekleyin
4. `/api/products` ile tüm ürünleri listeleyin

## 🎯 CQRS Pattern

Proje CQRS (Command Query Responsibility Segregation) pattern kullanır:

**Commands (Yazma):**
- CreateProductCommand
- UpdateProductCommand
- DeleteProductCommand
- RegisterCommand
- LoginCommand

**Queries (Okuma):**
- GetAllProductsQuery
- GetProductByIdQuery

## 🔐 Güvenlik

- **JWT Authentication**: Stateless token-based authentication
- **BCrypt**: Password hashing (cost factor: 10)
- **Input Validation**: DTO level validation
- **CORS**: Configurable cross-origin policy
- **HTTPS**: Production için önerilir

## 📊 Performans

- **Redis Caching**: 5 dakika TTL ile product cache
- **Async/Await**: Non-blocking I/O operations
- **Connection Pooling**: EF Core ve Redis için
- **Lazy Loading**: Disabled (explicit loading)

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 👨‍💻 Geliştirici

Onion Architecture, CQRS, JWT, ve Redis kullanılarak modern .NET standartlarına uygun olarak geliştirilmiştir.
