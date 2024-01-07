using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Media_Sphere
{
    internal class SQLiteDB
    {
        // Specify the fixed database name
        private const string DatabaseName = "MediaSphereDatabase";

        private readonly string _connectionString;

        public SQLiteDB()
        {
            // Specify the default directory where the database will be created
            string defaultDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the default directory and the fixed database name to get the full path
            string dbPath = Path.Combine(defaultDirectory, $"{DatabaseName}.db");

            // Set the connection string
            _connectionString = $"Data Source={dbPath};Version=3";

            InitializeDatabase(); // Call to create the FilePath table if it does not exist
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS FilePaths (Id INTEGER PRIMARY KEY AUTOINCREMENT, Path TEXT NOT NULL, Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP)", connection);
                command.ExecuteNonQuery();
            }
        }

        public void AddFilePath(string filePath)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("INSERT INTO FilePaths (Path) VALUES (@path)", connection);
                    command.Parameters.AddWithValue("@path", filePath);
                    command.ExecuteNonQuery();

                    MaintainTenEntries(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MaintainTenEntries(SQLiteConnection connection)
        {
            var countCommand = new SQLiteCommand("SELECT COUNT(*) FROM FilePaths", connection);
            long count = (long)countCommand.ExecuteScalar();

            if (count > 10)
            {
                var deleteCommand = new SQLiteCommand("DELETE FROM FilePaths WHERE Id IN (SELECT Id FROM FilePaths ORDER BY Timestamp ASC LIMIT (SELECT COUNT(*) - 10 FROM FilePaths))", connection);
                deleteCommand.ExecuteNonQuery();
            }
        }

        public List<string> GetFilePaths()
        {
            List<string> filePaths = new List<string>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT Path FROM FilePaths ORDER BY Timestamp DESC", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filePaths.Add(reader["Path"].ToString());
                    }
                }
            }

            return filePaths;
        }
    }
}

