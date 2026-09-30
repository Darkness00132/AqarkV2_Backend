# Aqark (عقارك) — Backend API

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-00B4A0?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-10-512BD4?logo=dotnet&logoColor=white)
![AWS S3](https://img.shields.io/badge/AWS-S3-FF9900?logo=amazons3&logoColor=white)

**Aqark** ("your property" in Arabic) is a real estate listings platform for the Egyptian market. This repository contains its backend REST API: brokers publish property ads (rent / sale) using a **credit-based system**, users browse, filter, review and report brokers, and admins handle verification. The platform is bilingual (Arabic / English) with Arabic-first user messaging and SEO-friendly Arabic slugs.

- 🌐 **Frontend** (separate Next.js app): https://aqark.vercel.app
- ⚙️ **API** (Swagger UI available in Development only): http://localhost:5188

---

## ✨ Features

| Area | What it does |
|---|---|
| **Authentication** | JWT access tokens (10 min) + rotating refresh tokens stored hashed in httpOnly cookies, email confirmation & password reset (via Resend), Google OAuth with a profile-completion step |
| **Ads** | Full CRUD with multi-image upload (converted to WebP on AWS S3), unique Arabic SEO slugs, rich filtering / sorting / pagination, in-memory caching per slug |
| **Credit system** | Ads cost credits — price depends on property type × ad type and the property's price tier; update costs decay with ad age; all transactions are logged |
| **Brokers** | Public broker profiles, verification requests reviewed by admins, user reports, and 1–5 star reviews with denormalized rating stats |
| **Audit trail** | Ad logs (create / update / delete) and credit logs (purchase / spend / refund / gift) for full traceability |
| **Platform hardening** | Global rate limiting (150 req/min per user or IP) + a strict 10 req / 5 min policy on auth endpoints, global exception handler returning RFC 7807 ProblemDetails, Serilog structured logging |

---

## 🏗️ Architecture

Clean Architecture with **Service → Repository → Unit of Work** (no CQRS / mediator). Dependencies point inward:

```
Backend.Api  ──►  Application  ──►  Domain
     │                ▲
     └───────►  Infrastructure (implements Application interfaces)
```

| Project | Responsibility |
|---|---|
| **Domain** | Core entities, enums, and domain services (e.g. the credit price calculator). No dependencies. |
| **Application** | Business logic: services, DTOs, AutoMapper profiles, custom validation attributes, options/settings, authorization policies, third-party abstractions (`IStorageService`, email, …) |
| **Infrastructure** | EF Core (`AppDbContext`, entity configurations, migrations), repositories + `UnitOfWork`, and third-party implementations: AWS S3 storage, Resend email, JWT token service, static Egypt location data |
| **Backend.Api** | Thin ASP.NET Core host: controllers, middleware, rate limiting, Swagger, composition root |

### Data & infrastructure

- **Database:** PostgreSQL (Npgsql), with domain enums mapped as native PostgreSQL enums
- **Identity:** ASP.NET Core Identity (`IdentityUser<Guid>` / `IdentityRole<Guid>`) with 4 seeded roles
- **File storage:** AWS S3 — images converted to WebP with ImageSharp, stored under `ads/{guid}.webp` / `avatar/{guid}.webp` with immutable cache headers
- **Email:** Resend (welcome, email verification, password reset)
- **Locations:** Egypt's governorates & cities as a validated static dataset (Arabic + English names)
- **Caching:** `IMemoryCache` for ad-by-slug reads (evicted on update / delete)

---

## 💰 The Credit System

Ads are not free — posting costs credits, simulating real monetization. Pricing is a pure domain service (`Domain/Services/AdCreditCalculator.cs`):

1. **Base cost** — a price matrix of *property type × ad type* (15 property types × rent / sale variants). Example: an apartment for rent costs **95 credits**, a hotel on sale-installment costs **205**.
2. **Price-tier bonus** — expensive properties cost more to list:
   - Sale: tiers at 500k / 1M / 2M / 5M / 10M EGP → **+0 / +14 / +28 / +48 / +72 / +100**
   - Rent: tiers at 2k / 5k / 10k / 25k / 50k EGP → same bonus ladder
3. **Update decay** — re-updating an ad gets cheaper as it ages: **100% of base cost in week 1, decaying to 25% by week 4+**. Price changes under 1,000 EGP are free.

---

## 🔄 Transaction Safety & Image Rollback

Ad creation is wrapped in a database transaction with compensating cleanup:

1. Validate city ↔ governorate consistency
2. Begin DB transaction
3. Upload images to S3
4. Compute credit cost and deduct from the broker's balance (fails with an "insufficient credits" error if short)
5. Insert the ad, write the ad log + negative credit log, save images
6. **Commit** — or, on any failure, **roll back the transaction *and* delete any already-uploaded S3 objects**

---

## 🧭 Domain Model

| Group | Entities |
|---|---|
| **Ads** | `Ad` (slug, price, space, rooms, type/state, governorate + city), `Image`, `AdLog`, `Governorate` / `City` |
| **Brokers** | `BrokerProfile` (credits balance, license, rating stats), `BrokerReview`, `BrokerReport`, `BrokerVerificationRequest` |
| **Users** | `User`, `Role`, `RefreshToken` (hashed, with IP tracking), `UserAccountSecurity` (block info, last login) |
| **Credits & payments** | `CreditsLog`, `CreditsPlan` (+ `PlanDiscount`), `Transaction`, `PaymentAttempt` |

Key enums (`AdType`, `PropertyType`, `AdState`, `PaymentStatus`, `ReportReason`, …) carry Arabic display translations. Roles: `User`, `Broker`, `Admin`, `SuperAdmin`.

---

## 🔌 API Surface

All routes are under `api/` (route template `api/[controller]`). Authorization uses policies: `UserOnly`, `BrokerOnly`, `AdminOnly`, `AdminOrBroker`, `SuperAdminOnly`.

### `api/Ads`
| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/api/Ads` | Public | Browse ads — filter by governorate, city, ad type, property type, finishing state, rooms, bathrooms, price & space ranges; sort by newest / price / space; paginated (default page size 12) |
| GET | `/api/Ads/{slug}` | Public | Ad details by SEO slug |
| GET | `/api/Ads/me` | BrokerOnly | The broker's own ads |
| GET | `/api/Ads/me/{id}` | BrokerOnly | Own ad details |
| POST | `/api/Ads` | BrokerOnly | Create ad — multipart form with images (1–5), charges credits |
| PUT | `/api/Ads/{id}` | BrokerOnly | Update ad (may charge credits per decay rules) |
| DELETE | `/api/Ads/{id}` | BrokerOnly | Delete ad |

### `api/Auth` *(rate-limited: 10 req / 5 min per IP)*
| Method | Route | Description |
|---|---|---|
| POST | `/register` | Register with email + password |
| POST | `/login` | Login (returns access token; sets refresh cookie) |
| GET | `/google` · `/google/callback` | Google OAuth flow |
| POST | `/completeProfile` | Finish OAuth signup (via short-lived `pending_user_id` cookie) |
| GET | `/confirmEmail` | Email confirmation link target |
| POST | `/forgotPassword` · `/resetPassword` | Password reset flow |
| POST | `/refresh` | Rotate refresh token (read from httpOnly cookie) and issue a new access token |
| DELETE | `/revokeCurrent` · `/revokeAll` | Revoke refresh token(s) |

Access tokens are returned in the JSON body; refresh tokens live in an httpOnly / Secure / SameSite=Strict cookie (30-day lifetime). Tokens are only issued to users with a confirmed email and completed profile. Login and refresh attempts record the client IP.

### `api/Brokers`
| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/api/Brokers` · `/api/Brokers/{slug}` | Public | Broker directory & profiles |
| GET / PUT | `/api/Brokers/me` | BrokerOnly | Own profile |

### `api/Reviews`
| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/api/Reviews/all/{slug}` · `/broker/{slug}` · `/{id}` | Public | Read reviews for a broker |
| POST / PUT / DELETE | `/api/Reviews/{brokerSlug}` / `/{id}` | UserOnly | One review per user per broker (1–5 stars) |

### `api/Users`
| Method | Route | Access |
|---|---|---|
| GET | `/api/Users/me` | Authorize |

---

## ⚙️ Configuration

`Backend.Api` reads these `appsettings.json` sections (bound to options classes in `Application/Settings`):

| Section | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | PostgreSQL connection string (dev default targets local DB `Aqark`) |
| `frontendUrl` | CORS origin + base URL for email links |
| `Jwt` | `Key` (256-bit), `Issuer`, `Audience`, `AccessTokenLifetimeMinutes`, `RefreshTokenLifetimeDays` |
| `EmailService:ApiKey` | Resend API key |
| `Google:ClientId` / `ClientSecret` | Google OAuth credentials |
| `S3Settings` | `Region`, `BucketName`, `AccessKey`, `SecretKey` |

> **Note:** all credential values in `appsettings*.json` are **placeholders** — never commit real secrets. Provide them via [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) locally and environment variables in production:
>
> ```bash
> dotnet user-secrets init --project Backend.Api
> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=Aqark;Username=postgres;Password=<your-password>" --project Backend.Api
> dotnet user-secrets set "Jwt:Key" "<256-bit-secret>" --project Backend.Api
> ```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL 14+ (local or hosted, e.g. Neon)

### Run

```bash
git clone https://github.com/Darkness00132/AqarkV2_Backend.git
cd AqarkV2_Backend

dotnet restore

# apply EF Core migrations (creates Identity + domain tables, seeds roles)
dotnet ef database update --project Infrastructure --startup-project Backend.Api

# configure secrets (or edit appsettings.Development.json)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=Aqark;Username=postgres;Password=<your-password>" --project Backend.Api

dotnet run --project Backend.Api
```

- HTTP: `http://localhost:5188` · HTTPS: `https://localhost:7152`
- **Swagger UI is served at the application root when running in Development** (XML doc comments included)
- Dev defaults assume PostgreSQL at `localhost`, database `Aqark`

### Docker

A multi-stage `Dockerfile` is included (base images `aspnet:10.0-noble` / `sdk:10.0-noble`, ports 8080 / 8081):

```bash
docker build -t aqark-api -f Backend.Api/Dockerfile .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="..." aqark-api
```

---

## 📁 Project Structure

```
Backend/
├── Backend.Api/            # ASP.NET Core host
│   ├── Controllers/        # Ads, Auth, Brokers, Reviews, Users
│   ├── Middleware/         # GlobalExceptionHandler (ProblemDetails)
│   ├── Program.cs          # DI composition, rate limiter, Serilog, Swagger
│   └── Dockerfile
├── Application/            # Business logic (no infrastructure deps)
│   ├── Services/           # AdService, AuthService, BrokerService, BrokerReviewService, UserService
│   ├── DTOs/  Interfaces/  # request/response contracts + repository & third-party abstractions
│   ├── Mapping/            # AutoMapper profiles
│   ├── Validators/         # custom validation attributes
│   ├── Common/             # filters, pagination
│   ├── Constants/          # authorization policies
│   └── Settings/           # JwtSettings, S3Settings
├── Domain/                 # Core model (no dependencies)
│   ├── Entities/           # Ads/, Brokers/, Users/, credits & payments
│   ├── Enums/              # with Arabic display names
│   └── Services/           # AdCreditCalculator (pure credit pricing engine)
└── Infrastructure/         # Data access & third parties
    ├── Presistance/        # AppDbContext + EF entity configurations, migrations
    ├── Repositories/       # per-area repositories + UnitOfWork
    ├── ThirdPartyService/  # JWT service, AWS S3, Resend email, Egypt locations
    └── DI/                 # registration extensions
```

---

## 📌 Design Notes

This project intentionally focuses on production-like concerns:

- **Business-driven logic** — the credit engine is a pure, testable domain service with no framework dependencies
- **Transactional integrity** — multi-step flows (upload → charge → persist) roll back completely, including remote files
- **Traceability** — every ad mutation and credit movement is journaled (`AdLog`, `CreditsLog`)
- **Arabic-first UX** — localized enums, error messages, slugs, and a bilingual location dataset
- **Defensive APIs** — layered rate limiting, strict auth policies, and consistent ProblemDetails errors
