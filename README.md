# 💊 Prescription Autofill and Search

A Windows Forms application built in C# that simplifies the process of storing, searching, and managing prescription records.  
The project is designed to help pharmacies and medical offices reduce manual data entry by supporting **OCR-based autofill**, **database integration**, and **fast record search**.

---

## 🚀 Features

- **OCR Autofill**  
  Extracts patient, doctor, and medicine information directly from scanned images or text using Tesseract OCR.

- **Database Integration**  
  Uses SQL Server to securely store prescription data (patients, doctors, medicines).

- **Duplicate Prevention**  
  Handles duplicate entries gracefully by checking constraints and showing friendly error messages.

- **Search Functionality**  
  Quickly search prescriptions by patient name, doctor, or medicine using a simple search form.

- **User-Friendly Interface**  
  Built with WinForms for a clean and easy-to-use experience.

- **Login Security**  
  Built-In login and sign up system that provides extra security.

---

## 🛠️ Tech Stack

- **Language:** C# (.NET Framework / WinForms)  
- **Database:** SQL Server (LocalDB / Express)  
- **OCR Engine:** [Tesseract OCR](https://github.com/charlesw/tesseract) (via .NET wrapper)  
- **IDE:** Visual Studio  

---

## 📂 Database Setup

This project uses **SQL Server** as the backend database.  
You’ll need to create a database and the required tables before running the application.

### 1. Create the Database
```sql
CREATE DATABASE LukeLoginTest;
```

### 2. Create the Tables
```sql
CREATE TABLE users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(100) NOT NULL
);
CREATE TABLE testingpatient (
    Patient NVARCHAR(100) NOT NULL,
    Doctor NVARCHAR(100),
    medicine NVARCHAR(100),
    PRIMARY KEY (Patient, Doctor, medicine)
);
```
---

## ⚙️ Setup & Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/lukenguyen9/LukeAutoFill.git
