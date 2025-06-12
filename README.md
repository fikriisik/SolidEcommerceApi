# 🔐 ASP.NET 7 Web API - JWT Authentication with PostgreSQL, Redis & Docker

Bu proje, **ASP.NET Core 7** kullanarak JWT tabanlı kimlik doğrulama ve refresh token sistemi sunar. Aşağıdaki özellikleri içerir:

---

## 🚀 Özellikler

* ✅ JWT access + refresh token sistemi
* ♻️ Refresh token retry limit kontrolü (Redis ile)
* 🔁 Refresh sonrası token yenileme & revoke işlemi
* 🛡 Role-based yetkilendirme (`[Authorize(Roles = "Admin")]`)
* 🧠 Redis ile token saklama & revoke takibi
* 🧾 PostgreSQL ile kullanıcı ve token veri yönetimi (EF Core)
* 🐳 Docker + Docker Compose desteği
* 📄 Swagger UI desteği (JWT auth test edilebilir)
* 📬 Postman koleksiyonu hazır
* 📈 Redis Insight ile monitoring
* 🛣 Nginx reverse proxy ile production ready yapı

---

## 🧱 Katmanlı Mimari & SOLID

Uygulama, **katmanlı mimari** prensibine ve **SOLID** yazılım geliştirme ilkelerine uygun şekilde yapılandırılmıştır:

| Katman/Folder   | Açıklama                                                                                               |
| --------------- | ------------------------------------------------------------------------------------------------------ |
| `Controllers/`  | HTTP isteklerini karşılayan uç noktalar (endpoints). API'nin dışa açılan yüzüdür.                      |
| `Services/`     | İş kuralları burada uygulanır. Controller'ların yönlendirdiği asıl iş mantığı bu katmanda yer alır.    |
| `Repositories/` | Veritabanı işlemleri (CRUD) burada soyutlanır. EF Core üzerinden `DbContext` ile veri yönetimi sağlar. |
| `DTOs/`         | Veri transfer nesneleri. İstemci ile veri alışverişinde kullanılan giriş/çıkış modelleri.              |
| `Helpers/`      | JWT üretimi, şifreleme, tarih/saat işlemleri gibi yardımcı (utility) sınıflar.                         |
| `Middlewares/`  | Custom middleware'lar (örn. token revoke kontrolü, hata yönetimi) burada bulunur.                      |
| `Models/`       | EF Core `Entity` sınıfları. Veritabanı tablolarını temsil eder.                                        |
| `Migrations/`   | EF Core tarafından oluşturulan migration dosyaları. Veritabanı şemasını oluşturur/günceller.           |
| `Validators/`   | FluentValidation kullanılarak DTO'lar için kuralların tanımlandığı katman.                             |
| `Data/`         | `DbContext`, seeding işlemleri ve veritabanı bağlantı yapılandırması burada yer alır.                  |
| `nginx/`        | Üretim ortamı için NGINX ters proxy yapılandırmaları (conf dosyaları vb.).                             |

> SOLID ilkeleri (Single Responsibility, Open/Closed, Liskov, Interface Segregation, Dependency Inversion) katmanlara yayılmıştır. Böylece kod daha sürdürülebilir, test edilebilir ve genişletilebilir hale gelir.

---

## ⚙️ Kurulum

### 1. Gerekli Araçlar

* [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
* [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)

### 2. Projeyi klonla

```bash
git clone hhttps://github.com/fikriisik/SolidEcommerceApi.git
cd SolidEcommerceApi
```

### 3. Docker ortamını başlat

```bash
docker-compose up -d --build
```

> Gerekirse kapatmak için:

```bash
docker-compose down
```

### 4. Migration ve Veritabanı Oluşturma

```bash
dotnet tool install --global dotnet-ef --version 7

# Migration oluştur (ilk kez yapılıyorsa)
dotnet ef migrations add InitialCreate

# Veritabanını oluştur
dotnet ef database update
```

---

## 📄 Swagger Kullanımı

Swagger UI ile API uç noktalarını kolayca test edebilirsiniz. Docker ortamında `localhost:8001` üzerinden yayınlanır.

### 🔗 Arayüze Erişim

```
http://localhost:5000/swagger
```

### 🔐 JWT ile Yetkilendirme

1. Swagger UI arayüzünde sağ üst köşedeki **Authorize** butonuna tıklayın.

2. Aşağıdaki formatta access token'ı girin:

   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```

3. "Authorize" butonuna basarak doğrulama sağlayın.

4. Artık `[Authorize]` anotasyonuna sahip endpoint’leri test edebilirsiniz.

---
---

## 📈 Redis Insight ile Monitoring

Token'ların Redis üzerinde nasıl tutulduğunu görselleştirmek ve izlemek için [Redis Insight](https://redis.com/redis-enterprise/redis-insight/) aracı kullanılabilir.

http://localhost:8002/
### Bağlantı Kurulumu

Redis container çalışır durumdayken, Redis Insight uygulamasına bağlanmak için:

* Host: `localhost`
* Port: `6379`

Arayüzde token verilerini `jwt:<userId>:refresh:<guid>` gibi key yapılarında izleyebilirsiniz.

---

## 🧪 Test & Geliştirme Notları

* Her build sonrası token’ların Redis'te düzgün oluştuğuna Redis Insight ile göz atabilirsiniz.
* `docker-compose logs -f` komutu ile servis loglarını takip edebilirsiniz.
* `dotnet watch run` ile local geliştirme yapabilirsiniz.

---

## 📜 Lisans

MIT Lisansı ile lisanslanmıştır.

---

Her türlü geri bildirim ve katkı için teşekkürler!
