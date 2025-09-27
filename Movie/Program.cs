//Author: Mobeen Mohammad Ali Raja
//Date: 12/09/2025



using Microsoft.Data.SqlClient; // You should automatically get this

class Program
{
    // Setting global connection string to be used to connect to sql database
    private const string ConnectionString =
        "Server=localhost,1433;Database=MovieDB;User Id=SA;Password=P455Word;TrustServerCertificate=True;";
    public static void Main(string[] args)
    {
        // Menu
        while (true)
        {
            Console.WriteLine("""
                ==============================
                ======= MOVIE DATABASE =======
                ==============================

                ==============================
                = 0. Exit                    =
                = 1. Create Movie            =
                = 2. Read Movie              =
                = 3. Update Movie            =
                = 4. Delete Movie            =
                = 5. Read All Movies         = 
                ==============================
                """
                );
            // Prompting menu selection
            Console.WriteLine("Enter Option: ");

            // Retrieving menu option
            string response = Console.ReadLine();

            int tempmovieId;


            if (response == "1") {

                Console.WriteLine("Enter Movie Title: ");
                string movieTitle = Console.ReadLine();

                Console.WriteLine("Enter Year: ");
                int year = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Description: ");
                string description = Console.ReadLine();

                tempmovieId = InsertMovie(movieTitle, year, description);

            } else if (response == "2"){

                Console.WriteLine("Enter movie you would like to read information about: ");
                string movieTitle = Console.ReadLine();

                Read(movieTitle);

            } else if (response == "3") {

                Console.WriteLine("Enter movie you would like to update description for");
                string movieTitle = Console.ReadLine();

                Console.WriteLine("Enter new description");
                string description = Console.ReadLine();

                UpdatingMovie(movieTitle, description);

            } else if (response == "4")
            {

                Console.WriteLine("Enter movie you would like to delete from database");
                string movieTitle = Console.ReadLine();
                DeleteMovie(movieTitle);

            }
            else if (response == "5") {

                Console.WriteLine("Reading all movies...\n");
                ReadAll();

            } else if (response == "0") {

                Console.WriteLine("Exiting program...");
                break;

            }



        }

        //Console.WriteLine("== Movie Database ==");

        // Create
        //int movieId = InsertMovie(
            //"Inception",
           // 2010,
            //"a mind-bending heist that takes place in people's dreams.");

        //Console.WriteLine($"New movie inserted at ID number {movieId}");

        // Read
        //GetMovie(movieId);

        // Update
        //UpdatingMovieDescription(movieId, "A science-fiction heist film by Christoper Nolan.");
        //GetMovie(movieId);

        // Delete
        //DeleteMovie(movieId);
        //GetMovie(movieId);

        //GetMovies();


    }


    private static void DeleteMovie(string movieTitle)
    {
        Console.WriteLine("Are you sure? Y/N: ");
        string? reply = Console.ReadLine().ToUpper();
        if (reply == "Y")
        {
            using var con = new SqlConnection(ConnectionString);
            con.Open();

            string sql = @"DELETE FROM Movies WHERE Title = @title;";
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@title", movieTitle);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? $"Movie {movieTitle} deleted\n" : "Movie not found\n");
        }
        else
        {
            Console.WriteLine("No action taken... returning to menu...\n");
        }

    }
    private static void UpdatingMovie(string movieTitle, string newDescription)
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = @"UPDATE Movies SET Description = @desc WHERE Title = @title;";
        using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@desc", newDescription);
        cmd.Parameters.AddWithValue("@title", movieTitle);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Movie updated.\n" : "Movie not found.\n"); // ternary statement being used
    }

    private static void Read(string movieTitle)
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = $"SELECT * FROM Movies WHERE Title = '{movieTitle}';";

        using var cmd = new SqlCommand(sql, con);

        using var reader = cmd.ExecuteReader();

        if (reader.Read() is false)
        {
            Console.WriteLine("No such movie exists");

        }
        else
        {

            while (reader.Read())
            {
                Console.WriteLine($"Movie {reader["id"]}: {reader["title"]} ({reader["yearOfRelease"]})");
                Console.WriteLine($""""
                Description:
                {reader["description"]}

                """");
            }
        }
    }

    private static void ReadAll()
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = "SELECT * FROM Movies;";

        using var cmd = new SqlCommand(sql, con);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"Movie {reader["id"]}: {reader["title"]} ({reader["yearOfRelease"]})");
            Console.WriteLine($""""
                Description:
                {reader["description"]}

                """");
        }
    }
    private static void GetMovies()
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = "SELECT * FROM Movies;";

        using var cmd = new SqlCommand(sql, con);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"Movie {reader["id"]}: {reader["title"]} ({reader["yearOfRelease"]})");
            Console.WriteLine($""""
                Description:
                {reader["description"]}

                """");
        }
    }


    private static void UpdatingMovieDescription(int movieId, string newDescription)
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = @"UPDATE Movies SET description = @desc WHERE id = @id;";
        using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@desc", newDescription);
        cmd.Parameters.AddWithValue("@id", movieId);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Movie updated.\n" : "Movie not found.\n"); // ternary statement being used
    }

    private static void GetMovie(int movieId)
    {
        using var con = new SqlConnection(ConnectionString);
        con.Open();

        string sql = @"SELECT * FROM Movies WHERE id = @id;";
        using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@id", movieId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Console.WriteLine($"Movie {reader["id"]}: {reader["title"]} ({reader["yearOfRelease"]})");
            Console.WriteLine($""""
                Description:
                {reader["description"]}

                """");
        }
        else
        {
            Console.WriteLine($"Id {movieId} is not found!\n");
        }
        con.Close();
    }

    private static int InsertMovie(string movieTitle, int movieYear, string movieDescription)
    {
        using var con = new SqlConnection(ConnectionString); // install Microsoft data sql client package and the line under "SqlConnection" will dissapear
        Console.WriteLine(con);
        con.Open();

        string sql = @"INSERT INTO Movies (Title, YearOfRelease, Description)
                        VALUES (@title, @year, @desc);
                        SELECT SCOPE_IDENTITY();";

        //string sq1l = $@"INSERT INTO Movies (Title, YearOfRelease, Description)
                        //VALUES ({movieTitle}, {movieYear}, {movieDescription});
                        //SELECT SCOPE_IDENTITY);";

        using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@title", movieTitle);
        cmd.Parameters.AddWithValue("@year", movieYear);
        cmd.Parameters.AddWithValue("@desc", movieDescription);

        return Convert.ToInt32(cmd.ExecuteScalar());
        Console.WriteLine("End of method");

    }


}