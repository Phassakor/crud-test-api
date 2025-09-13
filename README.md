# CRUD Test - Activity App

Simple CRUD application for **Activity**  
Stack: **.NET 8 Web API** + **MongoDB Atlas**
---

## Features

- **Activity CRUD** (create, update, soft delete)
- **Public API**: list + detail (status=published)
- **Manage API**: create/update/delete
- MongoDB Atlas (Free Tier) with proper indexes

---

## Tech Stack

- Backend: [.NET 8](https://dotnet.microsoft.com/) (C#)
- Database: [MongoDB Atlas Free Tier](https://www.mongodb.com/atlas/database)

---

## Getting Started (Backend)

### 1. Clone repo
git clone https://github.com/Phassakor/crud-test-api
cd crud-test-api/ActivityApi

### 2. Config MongoDB
edit appsettings.Development.json => ConnectionString

### 3. Run API
dotnet run
Seed data will be created automatically:
- Company Run 2025
- Volunteer Day
- Monthly Townhall (draft)

---

### 📡 API Endpoints
### Public
- GET /api/activities → List published activities (with pagination & search)
- GET /api/activities/{slug} → Get activity detail

### Manage
- POST /api/activities → Create new activity
- PUT /api/activities/{id} → Update activity
- DELETE /api/activities/{id} → Soft delete activity

---

### 📝 Example Payloads
### Create
{
  "title": "Green Earth Day 2025",
  "excerpt": "กิจกรรมปลูกต้นไม้รักษ์โลกประจำปี",
  "content": "รายละเอียดกิจกรรม...",
  "coverUrl": "https://picsum.photos/seed/green/800/400",
  "categories": ["environment"],
  "images": [
    { "url": "https://picsum.photos/seed/tree1/600/400", "order": 0 }
  ],
  "status": "published",
  "publishedAt": "2025-09-13T10:30:00Z"
}

### Update
{
  "title": "Green Earth Day 2025 (Updated)",
  "excerpt": "อัปเดตกิจกรรม",
  "content": "รายละเอียดใหม่...",
  "coverUrl": "https://picsum.photos/seed/green2/800/400",
  "categories": ["environment","community"],
  "images": [],
  "status": "published",
  "publishedAt": "2025-09-14T09:00:00Z"
}

---

### 🔍 Notes
- This project is for CRUD test/demo purpose only
- No authentication implemented (can be extended with JWT easily)

---

MIT © 2025 Phassakorn Suwannato
