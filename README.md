 # Inventory Asset Tracker
 ## Overview

The Inventory Asset Tracker is a secure client–server web application designed to help users manage and track assets such as equipment, devices, and other items.

The system supports both regular users and administrators, with a strong focus on security, role-based access control, and safe data handling.

### 🏗️ Architecture

The application follows a layered architecture:

Front-End (UI Layer):
ASP.NET Core MVC views
Handles user interaction and forms
Back-End (API Layer):
REST API controllers (.NET)
Handles business logic and processing
Security Layer:
Authentication
Authorization
Input validation
File upload protection
Database Layer:
SQL Server using Entity Framework Core
Stores users and assets

 The UI does not directly access the database, ensuring proper separation of concerns.

### 👥 User Roles

The system includes two roles:

User
Manage personal assets
Upload profile photo
Search, add, update, and delete assets
Admin
Access Admin panel
View all users and assets
Monitor system activity (logs)
### 🔑 Features
Core Features
User Registration and Login
Profile Management
Asset Management (CRUD)
Search functionality
File Upload (Profile Photo)
Admin Dashboard
### 🎯 Required Entry Points (Threat Surface)

The application includes all required entry points:

Login form (Authentication)
Search form (Assets)
File upload (Profile photo)
APIs (REST endpoints)
Admin panel (role-based access)
### 🔐 Security Features

The application implements multiple security controls:

Authentication
Cookie-based authentication
Password hashing using PasswordHasher
Authorization
[Authorize] protects authenticated routes
[Authorize(Roles = "Admin")] protects admin features
Input Validation
DTO-based validation
Server-side validation for all inputs
File Upload Security
Allowed file types: .jpg, .jpeg, .png, .webp
Maximum file size: 2MB
Files stored securely in /wwwroot/uploads
Database Security
Entity Framework Core used
Prevents SQL injection through parameterized queries
Logging & Monitoring
Admin logs interface for activity tracking
### 🛡️ OWASP Top 10 Coverage

The application addresses the following OWASP vulnerabilities:

A01: Broken Access Control
Role-based authorization prevents unauthorized access to admin features
A03: Injection
Entity Framework Core prevents SQL injection attacks
A05: Security Misconfiguration
File upload restrictions and controlled storage reduce risks
A07: Identification & Authentication Failures
Secure login with hashed passwords and session management
A09: Security Logging & Monitoring Failures
Admin logs allow tracking of system activity
### 🧠 STRIDE Threat Model
Threat	Example	Mitigation
Spoofing	Fake login attempt	Password hashing + authentication
Tampering	Modify asset data	Input validation + authorization
Repudiation	User denies actions	Logging system
Information Disclosure	Access sensitive data	Authentication + access control
Denial of Service	Large file uploads	File size limits
Elevation of Privilege	Access admin panel	Role-based access control
### ⚙️ Requirements

Before running the project, install:

.NET 8 SDK
Visual Studio 2022 (or newer)
SQL Server (LocalDB or full version)
SQL Server Management Studio (optional)
### 🚀 Setup Instructions
1. Clone the repository
git clone https://github.com/your-username/InventoryAssetTracker.git
2. Open the project

Open the .sln file in Visual Studio.

3. Configure the database

Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True"
}
4. Apply database migrations

Open Package Manager Console and run:

Update-Database

This will create the database and tables.

5. Run the application
Press F5 or click Run
The app will open in your browser
6. Create an Admin user
Register a new user in the app
Open SQL Server and run:
UPDATE Users
SET Role = 'Admin'
WHERE Username = 'your-username';
Logout and login again
### 📊 Assignment Requirements Covered

This project meets all required criteria:

Layered architecture ✔
UI separated from database ✔
API-based backend ✔
Required entry points ✔
Role-based access ✔
File upload security ✔
OWASP Top 10 implementation ✔
STRIDE threat modeling ✔
👨‍💻 Authors
Josiah Williams, Jeff David Tieng, Ricardo Gao
🏁 Final Notes

This application demonstrates a secure client–server architecture with a realistic threat surface. Security best practices such as authentication, authorization, validation, and logging have been implemented to align with industry standards.
