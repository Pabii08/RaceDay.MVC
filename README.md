# RaceDay Full-Stack Application (PROG6212 Part 3)

This repository contains the complete full-stack RaceDay platform, developed as part of the PROG6212 Portfolio of Evidence.

The system consists of a RESTful API (Part 2) and an ASP.NET Core MVC frontend (Part 3), fully containerized using Docker.

---

## Table of Contents
- [Technologies Used](#technologies-used)
- [User Roles](#user-roles)
- [Running the Application Locally](#running-the-application-locally)
- [Running with Docker](#running-with-docker)
- [Image Storage (Blob Simulation)](#image-storage-blob-simulation)
- [Testing & CI/CD](#testing--cicd)
- [YouTube Demonstration](#youtube-demonstration)

---

## Technologies Used
- **Backend API:** ASP.NET Core 8, Entity Framework Core, SQL Server.
- **Frontend:** ASP.NET Core 8 MVC, Bootstrap 5.
- **Authentication:** Session-based with BCrypt password hashing.
- **Containerization:** Docker & Docker Compose.
- **CI/CD:** GitHub Actions.

---

## User Roles

### Organiser
- Register and log in.
- Create events (with name, description, date, location, distance, type).
- Upload event banner images.
- View all enrolments for their specific events.
- Capture participant results.

### Participant
- Register and log in.
- View all upcoming events.
- Enrol in events by selecting a category.
- View a personal dashboard of their enrolments.
- View their personal race results.

---

## Running the Application Locally

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB)
- Visual Studio 2022

### Steps
1. Open the `RaceDay.FullStack.sln` solution in Visual Studio.
2. Right-click the Solution -> **Configure Startup Projects** -> Select **Multiple startup projects**.
3. Set both `Raceday.API` and `RaceDay.MVC` to **Start**.
4. Press **Run** (F5). 
5. The API will open Swagger at `https://localhost:7094/swagger`.
6. The MVC frontend will open at `https://localhost:xxxx`.

---

## Running with Docker

The application is fully containerized. The MVC app and the API run in separate containers and communicate over a Docker network.

### Steps
1. Ensure **Docker Desktop** is installed and running.
2. Open a terminal (PowerShell or Command Prompt) in the root directory (where `docker-compose.yml` is located).
3. Run the following command to build and start the containers:
   ```bash
   docker compose up --build