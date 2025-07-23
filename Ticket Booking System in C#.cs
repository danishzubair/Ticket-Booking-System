using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;

namespace AirlineBookingSystem
{
    public partial class MainForm : Form
    {
        private SQLiteConnection conn;
        private string connectionString = "Data Source=bookings.db;Version=3;";
        private int loggedInUserId = -1;

        public MainForm()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            conn = new SQLiteConnection(connectionString);
            conn.Open();

            // Create Users table
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Email TEXT NOT NULL
                )";
            using (SQLiteCommand cmd = new SQLiteCommand(createUsersTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Create Flights table
            string createFlightsTable = @"
                CREATE TABLE IF NOT EXISTS Flights (
                    FlightId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Airline TEXT NOT NULL,
                    Destination TEXT NOT NULL,
                    FlightDate TEXT NOT NULL,
                    Price REAL NOT NULL,
                    AvailableSeats INTEGER NOT NULL
                )";
            using (SQLiteCommand cmd = new SQLiteCommand(createFlightsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Create Hotels table
            string createHotelsTable = @"
                CREATE TABLE IF NOT EXISTS Hotels (
                    HotelId INTEGER PRIMARY KEY AUTOINCREMENT,
                    HotelName TEXT NOT NULL,
                    Location TEXT NOT NULL,
                    PricePerNight REAL NOT NULL,
                    AvailableRooms INTEGER NOT NULL
                )";
            using (SQLiteCommand cmd = new SQLiteCommand(createHotelsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Create Bookings table
            string createBookingsTable = @"
                CREATE TABLE IF NOT EXISTS Bookings (
                    BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    FlightId INTEGER,
                    HotelId INTEGER,
                    Seats INTEGER,
                    Nights INTEGER,
                    TotalCost REAL,
                    BookingDate TEXT,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId),
                    FOREIGN KEY(FlightId) REFERENCES Flights(FlightId),
                    FOREIGN KEY(HotelId) REFERENCES Hotels(HotelId)
                )";
            using (SQLiteCommand cmd = new SQLiteCommand(createBookingsTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Insert sample flights (Namibian context)
            string insertFlights = @"
                INSERT OR IGNORE INTO Flights (Airline, Destination, FlightDate, Price, AvailableSeats)
                VALUES
                    ('Air Namibia', 'Windhoek to Cape Town', '2025-07-25 14:00', 1500.00, 100),
                    ('Air Namibia', 'Windhoek to Johannesburg', '2025-07-26 16:00', 1800.00, 80)";
            using (SQLiteCommand cmd = new SQLiteCommand(insertFlights, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Insert sample hotels
            string insertHotels = @"
                INSERT OR IGNORE INTO Hotels (HotelName, Location, PricePerNight, AvailableRooms)
                VALUES
                    ('Hilton Windhoek', 'Windhoek', 200.00, 50),
                    ('Safari Court Hotel', 'Windhoek', 150.00, 60)";
            using (SQLiteCommand cmd = new SQLiteCommand(insertHotels, conn))
            {
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadFlights();
            LoadHotels();
        }

        private void LoadFlights()
        {
            conn.Open();
            string query = "SELECT FlightId, Airline || ' to ' || Destination || ' (' || FlightDate || ', $' || Price || ')' AS DisplayText FROM Flights";
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                flightListBox.DataSource = dt;
                flightListBox.DisplayMember = "DisplayText";
                flightListBox.ValueMember = "FlightId";
            }
            conn.Close();
        }

        private void LoadHotels()
        {
            conn.Open();
            string query = "SELECT HotelId, HotelName || ' (' || Location || ', $' || PricePerNight || '/night)' AS DisplayText FROM Hotels";
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                hotelListBox.DataSource = dt;
                hotelListBox.DisplayMember = "DisplayText";
                hotelListBox.ValueMember = "HotelId";
            }
            conn.Close();
        }

        // Form controls
        private ListBox flightListBox;
        private ListBox hotelListBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private TextBox emailTextBox;
        private TextBox seatsTextBox;
        private TextBox nightsTextBox;
        private CheckBox bookHotelCheckBox;
        private Button registerButton;
        private Button loginButton;
        private Button bookButton;
        private Button adminButton;
        private Label statusLabel;

        private void InitializeComponent()
        {
            this.Text = "Airline Booking System";
            this.Size = new System.Drawing.Size(800, 500);

            flightListBox = new ListBox();
            flightListBox.Location = new System.Drawing.Point(20, 20);
            flightListBox.Size = new System.Drawing.Size(300, 200);

            hotelListBox = new ListBox();
            hotelListBox.Location = new System.Drawing.Point(20, 240);
            hotelListBox.Size = new System.Drawing.Size(300, 200);

            bookHotelCheckBox = new CheckBox();
            bookHotelCheckBox.Text = "Book Hotel";
            bookHotelCheckBox.Location = new System.Drawing.Point(20, 220);
            bookHotelCheckBox.CheckedChanged += (s, e) => hotelListBox.Enabled = bookHotelCheckBox.Checked;

            usernameTextBox = new TextBox();
            usernameTextBox.Location = new System.Drawing.Point(350, 20);
            usernameTextBox.Size = new System.Drawing.Size(150, 25);
            usernameTextBox.PlaceholderText = "Username";

            passwordTextBox = new TextBox();
            passwordTextBox.Location = new System.Drawing.Point(350, 60);
            passwordTextBox.Size = new System.Drawing.Size(150, 25);
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.UseSystemPasswordChar = true;

            emailTextBox = new TextBox();
            emailTextBox.Location = new System.Drawing.Point(350, 100);
            emailTextBox.Size = new System.Drawing.Size(150, 25);
            emailTextBox.PlaceholderText = "Email";

            seatsTextBox = new TextBox();
            seatsTextBox.Location = new System.Drawing.Point(350, 140);
            seatsTextBox.Size = new System.Drawing.Size(150, 25);
            seatsTextBox.PlaceholderText = "Number of Seats";

            nightsTextBox = new TextBox();
            nightsTextBox.Location = new System.Drawing.Point(350, 180);
            nightsTextBox.Size = new System.Drawing.Size(150, 25);
            nightsTextBox.PlaceholderText = "Number of Nights";
            nightsTextBox.Enabled = false;
            bookHotelCheckBox.CheckedChanged += (s, e) => nightsTextBox.Enabled = bookHotelCheckBox.Checked;

            registerButton = new Button();
            registerButton.Text = "Register";
            registerButton.Location = new System.Drawing.Point(350, 220);
            registerButton.Size = new System.Drawing.Size(150, 30);
            registerButton.Click += RegisterButton_Click;

            loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Location = new System.Drawing.Point(350, 260);
            loginButton.Size = new System.Drawing.Size(150, 30);
            loginButton.Click += LoginButton_Click;

            bookButton = new Button();
            bookButton.Text = "Book Now";
            bookButton.Location = new System.Drawing.Point(350, 300);
            bookButton.Size = new System.Drawing.Size(150, 30);
            bookButton.Click += BookButton_Click;

            adminButton = new Button();
            adminButton.Text = "Admin Panel";
            adminButton.Location = new System.Drawing.Point(350, 340);
            adminButton.Size = new System.Drawing.Size(150, 30);
            adminButton.Click += AdminButton_Click;

            statusLabel = new Label();
            statusLabel.Location = new System.Drawing.Point(20, 450);
            statusLabel.Size = new System.Drawing.Size(750, 30);
            statusLabel.Text = "Status: Not logged in";

            this.Controls.Add(flightListBox);
            this.Controls.Add(hotelListBox);
            this.Controls.Add(bookHotelCheckBox);
            this.Controls.Add(usernameTextBox);
            this.Controls.Add(passwordTextBox);
            this.Controls.Add(emailTextBox);
            this.Controls.Add(seatsTextBox);
            this.Controls.Add(nightsTextBox);
            this.Controls.Add(registerButton);
            this.Controls.Add(loginButton);
            this.Controls.Add(bookButton);
            this.Controls.Add(adminButton);
            this.Controls.Add(statusLabel);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string email = emailTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields.", "Error");
                return;
            }

            conn.Open();
            string query = "INSERT INTO Users (Username, Password, Email) VALUES (@username, @password, @email)";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password); // In production, hash passwords
                cmd.Parameters.AddWithValue("@email", email);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registration successful!", "Success");
                }
                catch
                {
                    MessageBox.Show("Username already exists.", "Error");
                }
            }
            conn.Close();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Error");
                return;
            }

            conn.Open();
            string query = "SELECT UserId FROM Users WHERE Username = @username AND Password = @password";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    loggedInUserId = Convert.ToInt32(result);
                    statusLabel.Text = $"Status: Logged in as {username}";
                    MessageBox.Show("Login successful!", "Success");
                }
                else
                {
                    MessageBox.Show("Invalid credentials.", "Error");
                }
            }
            conn.Close();
        }

        private void BookButton_Click(object sender, EventArgs e)
        {
            if (loggedInUserId == -1)
            {
                MessageBox.Show("Please log in first.", "Error");
                return;
            }

            if (flightListBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a flight.", "Error");
                return;
            }

            if (!int.TryParse(seatsTextBox.Text, out int seats) || seats <= 0)
            {
                MessageBox.Show("Please enter a valid number of seats.", "Error");
                return;
            }

            int? hotelId = bookHotelCheckBox.Checked && hotelListBox.SelectedValue != null ? Convert.ToInt32(hotelListBox.SelectedValue) : (int?)null;
            int nights = 0;
            if (bookHotelCheckBox.Checked)
            {
                if (!int.TryParse(nightsTextBox.Text, out nights) || nights <= 0)
                {
                    MessageBox.Show("Please enter a valid number of nights.", "Error");
                    return;
                }
            }

            int flightId = Convert.ToInt32(flightListBox.SelectedValue);
            double totalCost = 0;

            conn.Open();

            // Check flight seats
            string flightQuery = "SELECT Airline, Destination, FlightDate, Price, AvailableSeats FROM Flights WHERE FlightId = @flightId";
            string flightDetails = "";
            double flightPrice = 0;
            using (SQLiteCommand cmd = new SQLiteCommand(flightQuery, conn))
            {
                cmd.Parameters.AddWithValue("@flightId", flightId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int availableSeats = reader.GetInt32(4);
                        if (seats > availableSeats)
                        {
                            MessageBox.Show($"Only {availableSeats} seats available.", "Error");
                            conn.Close();
                            return;
                        }
                        flightDetails = $"{reader.GetString(0)} to {reader.GetString(1)} on {reader.GetString(2)}";
                        flightPrice = reader.GetDouble(3) * seats;
                        totalCost += flightPrice;
                    }
                }
            }

            // Check hotel rooms (if selected)
            string hotelDetails = "";
            double hotelPrice = 0;
            if (hotelId.HasValue)
            {
                string hotelQuery = "SELECT HotelName, Location, PricePerNight, AvailableRooms FROM Hotels WHERE HotelId = @hotelId";
                using (SQLiteCommand cmd = new SQLiteCommand(hotelQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@hotelId", hotelId.Value);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int availableRooms = reader.GetInt32(3);
                            if (1 > availableRooms) // Assuming 1 room per booking
                            {
                                MessageBox.Show($"No rooms available at {reader.GetString(0)}.", "Error");
                                conn.Close();
                                return;
                            }
                            hotelDetails = $"{reader.GetString(0)} in {reader.GetString(1)} for {nights} nights";
                            hotelPrice = reader.GetDouble(2) * nights;
                            totalCost += hotelPrice;
                        }
                    }
                }
            }

            // Create booking
            string bookingQuery = "INSERT INTO Bookings (UserId, FlightId, HotelId, Seats, Nights, TotalCost, BookingDate) VALUES (@userId, @flightId, @hotelId, @seats, @nights, @totalCost, @bookingDate)";
            using (SQLiteCommand cmd = new SQLiteCommand(bookingQuery, conn))
            {
                cmd.Parameters.AddWithValue("@userId", loggedInUserId);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                cmd.Parameters.AddWithValue("@hotelId", hotelId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@seats", seats);
                cmd.Parameters.AddWithValue("@nights", nights);
                cmd.Parameters.AddWithValue("@totalCost", totalCost);
                cmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                cmd.ExecuteNonQuery();
            }

            // Update flight seats
            string updateFlightQuery = "UPDATE Flights SET AvailableSeats = AvailableSeats - @seats WHERE FlightId = @flightId";
            using (SQLiteCommand cmd = new SQLiteCommand(updateFlightQuery, conn))
            {
                cmd.Parameters.AddWithValue("@seats", seats);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                cmd.ExecuteNonQuery();
            }

            // Update hotel rooms (if selected)
            if (hotelId.HasValue)
            {
                string updateHotelQuery = "UPDATE Hotels SET AvailableRooms = AvailableRooms - 1 WHERE HotelId = @hotelId";
                using (SQLiteCommand cmd = new SQLiteCommand(updateHotelQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@hotelId", hotelId.Value);
                    cmd.ExecuteNonQuery();
                }
            }

            // Show receipt
            string receipt = $"Booking Receipt\n\nFlight: {flightDetails}\nSeats: {seats}\nFlight Cost: ${flightPrice:F2}\n";
            if (hotelId.HasValue)
            {
                receipt += $"Hotel: {hotelDetails}\nHotel Cost: ${hotelPrice:F2}\n";
            }
            receipt += $"Total Cost: ${totalCost:F2}\nBooking Date: {DateTime.Now:yyyy-MM-dd HH:mm}";
            MessageBox.Show(receipt, "Booking Confirmation");

            conn.Close();
            LoadFlights();
            LoadHotels();
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Admin panel not implemented in this demo. Use SQLite DB browser to manage flights and hotels.", "Info");
            // In a full implementation, open a form to add/remove flights and hotels
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}