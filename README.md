# RaceDay Full-Stack Application (PROG6212 Part 3)

This project is the complete full-stack RaceDay platform, developed for the PROG6212 Portfolio of Evidence. It consists of a backend RESTful API and a frontend ASP.NET Core MVC application, both fully containerized using Docker.

**Important Note for the Lecturer:** 
Because this is a full-stack application, the code is split into two separate repositories. 
- **API Repository:** [https://github.com/Pabii08/RaceDay.API](API-GITHUB-LINK)
- **MVC Repository:** [https://github.com/Pabii08/RaceDay.MVC](MVC-GITHUB-LINK)

---

## Technologies Used
- **Backend API:** ASP.NET Core 8, Entity Framework Core, SQL Server.
- **Frontend:** ASP.NET Core 8 MVC, Bootstrap 5.
- **Authentication:** Session-based with BCrypt password hashing.
- **Containerization:** Docker & Docker Compose.
- **CI/CD:** GitHub Actions

---

## User Roles

### Organiser
- Register and log in.
- Create events (with name, description, date, location, distance, type).
- Upload event banner images (Simulated Blob Storage).
- View all enrolments for their specific events.
- Capture participant results.

### Participant
- Register and log in.
- View all upcoming events.
- Enrol in events by selecting a category.
- View a personal dashboard of their enrolments.
- View their personal race results.

---

## Running the Application Locally (Without Docker)

To run the full application locally, you must run both projects at the same time.

**Step 1: Start the API**
1. Open the API repository in Visual Studio.
2. Open the Package Manager Console and run `Update-Database` to ensure the SQL database is created.
3. Press **Run** (F5). Swagger will open at `https://localhost:7094/swagger`. Keep this running.

**Step 2: Start the MVC App**
1. Open the MVC repository in a second Visual Studio window.
2. Press **Run** (F5). The frontend will open in your browser (e.g., `https://localhost:7024`).
3. You can now register, log in, and use the application.

---

## Running with Docker

The application is fully containerized. The MVC app and the API run in separate containers and communicate over a Docker network.

### Prerequisites
- Docker Desktop installed and running.

### Steps
1. Ensure both repositories are cloned into the same parent folder on your computer.
2. Open a terminal (PowerShell or Command Prompt) in the root directory (where the `docker-compose.yml` file is located).
3. Run the following command to build and start the containers:
   ```bash
   docker compose up --build