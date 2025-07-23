# Airline-Booking-System

Overview

The Airline Booking System is a C# desktop application developed for a Namibian business to manage airline ticket bookings with optional hotel accommodations. Built using .NET with Windows Forms and SQLite, it allows users to register, log in, browse available flights, book tickets, and add hotel stays. After booking, a receipt is displayed on-screen with flight and hotel details (if selected) and the total cost. The system includes a basic admin panel placeholder for managing flights and hotels, designed for simplicity and reliability in a local business context.

Project Description

This system was created to streamline ticket booking for a Namibian business (e.g., a travel agency or Air Namibia partner). It supports:





User Management: Register and log in to book tickets.



Flight Booking: Browse and book flights with details like airline, destination, date, price, and available seats.



Hotel Booking: Optionally book a hotel room during the purchase process, selecting hotel, location, and number of nights.



Receipt Display: Show a detailed receipt on-screen after booking, including flight, hotel (if selected), and total cost.



Admin Functionality: Placeholder for managing flights and hotels (add/remove), to be extended as needed.

The system uses SQLite for data storage, ensuring lightweight deployment suitable for a small business. It was designed to meet the needs of a real-world Namibian client, focusing on usability and core functionality.

Features





User Registration/Login: Secure user accounts with username, password, and email.



Flight Selection: Browse available flights (e.g., Air Namibia routes) and book seats.



Hotel Booking Option: Add a hotel stay with customizable nights during booking.



Receipt Display: Shows flight and hotel details with total cost in a message box after purchase.



Admin Placeholder: Supports future admin panel for managing flights and hotels.



SQLite Integration: Stores users, flights, hotels, and bookings in a local database.

Technologies Used





C# .NET: Core programming language and framework.



Windows Forms: For the desktop user interface.



SQLite: Lightweight database for storing user, flight, hotel, and booking data.



System.Data.SQLite: ADO.NET provider for SQLite integration.

Installation





Clone the Repository:

git clone https://github.com/danishzubair/airline-booking-system.git



Navigate to the Project Directory:

cd airline-booking-system



Set Up the Environment:





Install .NET Framework 4.8 or later.



Install Visual Studio Community (or another C# IDE).



Install the System.Data.SQLite NuGet package:

dotnet add package System.Data.SQLite



Build and Run:





Open the project in Visual Studio, build, and run.



Alternatively, compile and run via command line:

csc AirlineBookingSystem.cs
AirlineBookingSystem.exe



The database (bookings.db) is created automatically on first run.

Usage





Run the Application:





Launch the executable or run through Visual Studio.



Register or Log In:





Enter a username, password, and email to register.



Log in with existing credentials.



Book a Flight:





Select a flight from the list (e.g., “Air Namibia to Windhoek”).



Enter the number of seats.



Optionally check “Book Hotel,” select a hotel, and enter the number of nights.



Click “Book Now” to confirm.



View Receipt:





After booking, a message box displays the receipt with flight details, hotel details (if selected), and total cost.



Admin Tasks:





Click “Admin Panel” (placeholder; use SQLite DB browser to manage flights/hotels for now).

Repository Structure

airline-booking-system/
├── AirlineBookingSystem.cs  # Main C# source code
├── bookings.db             # SQLite database (created on first run)
└── README.md              # This file

Contributing

This is a personal portfolio project. Contributions are not expected, but feedback is welcome. Contact me via GitHub Issues or email.

License

This project is for demonstration purposes and not licensed for public use. All rights reserved by Danish Zubair.

Contact





Email: danish347291@gmail.com



Phone: +44 (0) 7405 877 612



GitHub: danishzubair



LinkedIn: Your LinkedIn Profile (Update with your actual profile)



Portfolio: danishzubair.github.io



Built by Danish Zubair, Software Engineering Student at University of Bradford, for a Namibian business.
