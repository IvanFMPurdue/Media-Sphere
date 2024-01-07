using System;
using System.Data.SQLite;
using System.IO;

namespace Media_Sphere
{
    public class SQLiteSchema
    {
        private static string DatabaseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Media_Sphere", "Database");
        private const string DatabaseFileName = "SQLiteDB.sqlite";
        private static string ConnectionString = "Data Source=" + Path.Combine(DatabaseFolderPath, DatabaseFileName) + ";Version=3;";

        public SQLiteSchema()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!Directory.Exists(DatabaseFolderPath))
            {
                Directory.CreateDirectory(DatabaseFolderPath);
            }

            if (!File.Exists(Path.Combine(DatabaseFolderPath, DatabaseFileName)))
            {
                SQLiteConnection.CreateFile(Path.Combine(DatabaseFolderPath, DatabaseFileName));

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string createTableQuery = "CREATE TABLE IF NOT EXISTS RecentFiles (ID INTEGER PRIMARY KEY AUTOINCREMENT, FilePath TEXT UNIQUE)";
                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void AddRecentFile(string filePath)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT OR IGNORE INTO RecentFiles (FilePath) VALUES (@FilePath)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    command.ExecuteNonQuery();
                }
            }
        }

        public string[] GetRecentFiles()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT FilePath FROM RecentFiles ORDER BY ID DESC LIMIT 10";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var files = new System.Collections.Generic.List<string>();
                        while (reader.Read())
                        {
                            files.Add(reader.GetString(0));
                        }
                        return files.ToArray();
                    }
                }
            }
        }
    }
}
