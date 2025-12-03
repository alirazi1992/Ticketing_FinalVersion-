# Ticketinground2 🎫

A modern, Farsi (RTL)-first IT support ticketing dashboard built with **Next.js**, **Tailwind CSS**, and **TypeScript** using **Clean Architecture**.  
Includes a multi-role system (**Client**, **Admin**, **Technician**) for tracking, managing, and replying to support requests in real time.

---

## 📑 Table of Contents

1. [Features](#-features)  
2. [Tech Stack](#-tech-stack)  
3. [Getting Started](#-getting-started)  
   - [Prerequisites](#prerequisites)  
   - [Installation](#installation)  
   - [Environment Variables](#environment-variables)  
   - [Running Locally](#running-locally)  
4. [Architecture](#-architecture)  
5. [Usage](#-usage)  
6. [Roadmap](#-roadmap)  
7. [Contributing](#-contributing)  
8. [License](#-license)  

---

## ✨ Features

- Multi-step **ticket creation** (with dynamic categories & subcategories)  
- Full **RTL (Farsi)** UI support  
- **Role-based dashboards**: Client / Admin / Technician  
- Technician **reply & messaging** system  
- Admin **ticket assignment & management**  
- **Dark / Light mode** toggle  
- **Farsi / English language switching**  
- Secure form handling & validation with **Yup**  
- Fully responsive, **mobile-friendly UI**  
- Modern **glassmorphism design**  
- **WebSocket-ready** for real-time updates  

---

## 🛠 Tech Stack

- **Framework**: [Next.js](https://nextjs.org/) (v14+)  
- **Styling**: [Tailwind CSS](https://tailwindcss.com/)  
- **Language**: TypeScript  
- **Architecture**: Clean Architecture  
- **Forms**: React Hook Form + Yup  

---

## 🚀 Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) (v16 or newer recommended)  
- npm / pnpm / yarn  
- (Optional) Docker & docker-compose  

---

### Installation

```bash
# Clone repository
git clone https://github.com/alirazi1992/Ticketinground2-.git
cd Ticketinground2-

# Install dependencies
npm install
# or
pnpm install
# or
yarn install
```
---

## 🏗 Architecture

This project follows **Clean Architecture**:

- **App / UI Layer** — pages, components, layouts, presentation logic

- **Domain Layer** — ticket models, validation schemas, business rules

- **Data Layer** — API calls, WebSocket services, persistence logic

- **Shared Layer** — hooks, utils, global styles, constants

This makes the codebase **scalable, testable, and easy to maintain**.

---

## 📌 Usage

- Log in as **Client** → create tickets, view status, message technicians

- Log in as **Admin** → assign tickets, manage users, monitor dashboard

- Log in as **Technician** → view assigned tickets, reply, update status

- Toggle **Dark / Light** theme

- Switch **Farsi ↔ English**

---

## 🗺 Roadmap

✅ Multi-role dashboards (Client / Admin / Technician)

✅ Multi-step ticket forms with validation

🔲 Real-time WebSocket updates (live notifications)

🔲 Admin analytics dashboard (ticket stats, response time, etc.)

🔲 UI/UX polishing & animations

🔲 Deployment & CI/CD pipelines

🔲 Add more languages beyond Farsi/English

---

## 🧭 Backend (C#) Preview

A lightweight **ASP.NET Core Minimal API** backend is included under `backend/src/Ticketing.Api` to serve the dashboards. It ships with an in-memory store, seeded tickets, and role-aware reply/assignment endpoints so you can iterate quickly.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Running locally

```bash
# Restore dependencies
dotnet restore backend/src/Ticketing.Api/Ticketing.Api.csproj

# Run the API (listens on http://localhost:5000 by default)
dotnet run --project backend/src/Ticketing.Api/Ticketing.Api.csproj
```

### Available endpoints

- `GET /api/tickets` — list all tickets
- `GET /api/tickets/{id}` — fetch ticket details
- `POST /api/tickets` — create a ticket
- `POST /api/tickets/{id}/assign` — assign to a technician
- `POST /api/tickets/{id}/reply` — add a role-tagged reply
- `POST /api/tickets/{id}/status` — update workflow status

Swagger UI is enabled by default at `/swagger` for quick exploration and manual testing.
