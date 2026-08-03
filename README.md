# Aqark 🏠

A real estate backend platform that manages property listings, brokers, and reviews with a credit-based system.

The project focuses on handling real-world scenarios like transactional safety, file management, and business-driven logic.

---

## 🚀 Features

* Authentication (JWT + Refresh Tokens + Google OAuth)
* Ads management with image uploads
* Credit system for posting and updating ads
* Broker profiles & reviews
* Filtering & pagination

---

## 🏗️ Architecture

Follows Clean Architecture:

* Application → business logic (services)
* Domain → core entities & rules
* Infrastructure → data access & external services

Uses:

* Repository pattern
* Unit of Work
* Dependency Injection

---

## ⚙️ Core Logic Highlights

### Credit-based Ads System

Ads are not free — users spend credits when:

* Creating an ad
* Updating price (dynamic cost calculation)

This simulates real-world monetization logic.

---

### Transaction Safety

Critical flows are wrapped in transactions.

Example: Creating an Ad

* Upload images
* Deduct credits
* Save ad
* Log actions
* Rollback everything if any step fails

Includes cleanup of uploaded files on failure.

---

### Image Handling

* External storage abstraction
* Multiple uploads
* Safe deletion on rollback
* Max images validation (1–5 per ad)

---

### Slug System

SEO-friendly Arabic slugs:

* Normalized text (remove diacritics)
* Cleaned characters
* Unique identifier appended

---

### Audit & Activity Tracking

The system tracks critical actions for consistency and traceability:

- **Ad Logs** → track ad lifecycle (create, update, delete)
- **Credit Logs** → track all credit transactions (spend, gift)

This enables:
- Debugging and issue tracing  
- Financial transparency  
- Reconstructing user actions history

---

## 🛠️ Tech Stack

* ASP.NET Core
* Entity Framework Core
* ASP.NET Identity
* AutoMapper
* JWT

---

## 🧪 Run

```bash id="run-steps"
git clone https://github.com/Darkness00132/AqarkV2_Backend/tree/master
cd aqark
dotnet restore
dotnet run
```

---

## 📌 Notes

This project focuses on:

* Writing production-like business logic
* Handling edge cases (credits, file rollback)
* Keeping services clean and maintainable
