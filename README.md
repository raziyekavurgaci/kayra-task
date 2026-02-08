# Product API

Ürün yönetimi için geliştirilmiş RESTful API. Temel CRUD işlemlerini destekler.

## 🛠️ Kullanılan Teknolojiler

- **.NET 10.0** - ASP.NET Core Web API
- **C#** - Programlama dili
- **PostgreSQL** - Veritabanı
- **Entity Framework Core** - ORM
- **Swagger** - API dokümantasyonu

## 📁 Proje Yapısı

```
ProductApi/
├── Controllers/          # API endpoint'leri
├── Services/            # İş mantığı katmanı
├── Repositories/        # Veritabanı işlemleri
├── Models/              # Veritabanı modelleri
├── DTOs/                # Veri transfer objeleri
├── Data/                # Veritabanı context
└── Program.cs           # Uygulama başlangıcı
```

## 🏗️ Mimari

Proje **katmanlı mimari** prensiplerine göre geliştirilmiştir:

- **Controller Katmanı**: HTTP isteklerini karşılar
- **Service Katmanı**: İş mantığı ve validasyon
- **Repository Katmanı**: Veritabanı işlemleri
- **Model/DTO Katmanı**: Veri yapıları

**SOLID Prensipleri:**
- Dependency Injection kullanılmıştır
- Her katman tek sorumluluk prensibine uyar
- Interface'ler ile gevşek bağlılık sağlanmıştır

## 📋 Gereksinimler

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- PostgreSQL kullanıcı adı ve şifresi

## 🚀 Kurulum

### 1. Projeyi Klonlayın

```bash
git clone <repository-url>
cd kayra-task
```

### 2. PostgreSQL Veritabanını Oluşturun

PostgreSQL'de `kayra_task` veritabanını oluşturun:

```sql
CREATE DATABASE kayra_task;
```

### 3. Bağlantı Ayarlarını Yapılandırın

`api/ProductApi/appsettings.json` dosyasında PostgreSQL bağlantı bilgilerinizi güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kayra_task;Username=postgres;Password=yourpassword"
  }
}
```

### 4. Paketleri Yükleyin

```bash
cd api/ProductApi
dotnet restore
```

### 5. Veritabanı Migration'ını Çalıştırın

```bash
dotnet ef database update
```

Bu komut `Products` tablosunu otomatik olarak oluşturacaktır.

## ▶️ Çalıştırma

```bash
dotnet run
```

Uygulama varsayılan olarak şu adreste çalışacaktır:
```
http://localhost:5044
```

Swagger UI için tarayıcınızda şu adresi açın:
```
http://localhost:5044
```

## 📚 API Endpoint'leri

### Tüm Ürünleri Listele
```http
GET /api/products
```

**Cevap:**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "Gaming laptop",
    "price": 15000.00,
    "stock": 10,
    "createdDate": "2026-02-08T13:00:00Z",
    "updatedDate": null
  }
]
```

### Tek Ürün Getir
```http
GET /api/products/{id}
```

**Cevap:**
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "Gaming laptop",
  "price": 15000.00,
  "stock": 10,
  "createdDate": "2026-02-08T13:00:00Z",
  "updatedDate": null
}
```

### Yeni Ürün Ekle
```http
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "description": "Gaming laptop",
  "price": 15000.00,
  "stock": 10
}
```

**Cevap:** `201 Created`

### Ürün Güncelle
```http
PUT /api/products/{id}
Content-Type: application/json

{
  "name": "Laptop Pro",
  "description": "Updated description",
  "price": 18000.00,
  "stock": 5
}
```

**Cevap:** `200 OK`

### Ürün Sil
```http
DELETE /api/products/{id}
```

**Cevap:** `204 No Content`

## 🧪 Test

Swagger UI kullanarak API'yi test edebilirsiniz:

1. Uygulamayı çalıştırın: `dotnet run`
2. Tarayıcıda açın: `http://localhost:5044`
3. Swagger arayüzünden endpoint'leri test edin

## 🔧 Geliştirme

### Yeni Migration Oluşturma

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Build

```bash
dotnet build
```

### Test

```bash
dotnet test
```

## 📝 Özellikler

- ✅ RESTful API tasarımı
- ✅ Asenkron programlama (async/await)
- ✅ Entity Framework Core ile veritabanı yönetimi
- ✅ Katmanlı mimari (Controller-Service-Repository)
- ✅ Dependency Injection
- ✅ DTO pattern ile veri transferi
- ✅ Input validasyonu
- ✅ Exception handling
- ✅ Swagger/OpenAPI dokümantasyonu
- ✅ PostgreSQL veritabanı desteği

## 🗂️ Veritabanı Şeması

### Products Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| Id | INTEGER | Primary Key, otomatik artan |
| Name | VARCHAR(200) | Ürün adı (zorunlu) |
| Description | VARCHAR(1000) | Ürün açıklaması (opsiyonel) |
| Price | DECIMAL(18,2) | Ürün fiyatı (zorunlu) |
| Stock | INTEGER | Stok miktarı (zorunlu) |
| CreatedDate | TIMESTAMP | Oluşturulma tarihi (otomatik) |
| UpdatedDate | TIMESTAMP | Güncellenme tarihi (opsiyonel) |

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
