using Microsoft.Data.Sqlite;

namespace selenium_xunit_reqnroll_framework.Utilities;

public class UserCredential
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public static class UserCredentialDb
{
    // Use the project directory as the DB location
    private static readonly string DbPath = Path.Combine(Directory.GetCurrentDirectory(), "usercredentials.db");

    public static void InitializeDb()
    {
        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();
        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL
        );";
        tableCmd.ExecuteNonQuery();
    }

    public static void InsertUser(UserCredential user)
    {
        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();
        using var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = "INSERT INTO Users (Username, Email, Password) VALUES ($username, $email, $password);";
        insertCmd.Parameters.AddWithValue("$username", user.Username);
        insertCmd.Parameters.AddWithValue("$email", user.Email);
        insertCmd.Parameters.AddWithValue("$password", user.Password);
        insertCmd.ExecuteNonQuery();
    }

    public static List<UserCredential> GetAllUsers()
    {
        var users = new List<UserCredential>();
        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();
        using var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT Username, Email, Password FROM Users;";
        using var reader = selectCmd.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new UserCredential
            {
                Username = reader.GetString(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2)
            });
        }
        return users;
    }

    public static UserCredential GetRandomUser()
    {
        var users = GetAllUsers();
        if (users.Count == 0) throw new InvalidOperationException("No users in DB");
        var rand = new Random();
        return users[rand.Next(users.Count)];
    }

    public static void DeleteUserByUsername(string username)
    {
        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();
        using var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Users WHERE Username = $username;";
        deleteCmd.Parameters.AddWithValue("$username", username);
        deleteCmd.ExecuteNonQuery();
    }

    public static void DeleteUsersByUsernames(IEnumerable<string> usernames)
    {
        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();
        foreach (var username in usernames)
        {
            using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Users WHERE Username = $username;";
            deleteCmd.Parameters.AddWithValue("$username", username);
            deleteCmd.ExecuteNonQuery();
        }
    }
}
