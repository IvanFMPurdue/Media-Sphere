using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;//need nuget package System.Data.SQLite

namespace Media_Sphere
{
    internal class SQLiteExample
    {
        private readonly string _connectionString;//Connection to sql database

        public SQLiteExample(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Version=3";
            InitializeDatabase();//call to create the FilePath table if it does not exist
        }

        private void InitializeDatabase()//Creates required database table
        {
            using (var connection = new SQLiteConnection(_connectionString))//create new sqlite connection
            {
                connection.Open();
                //sql command to create a new table for file paths
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
                    //sql command to insert a new file path into the FilePaths table
                    var command = new SQLiteCommand("INSERT INTO FilePaths (Path) VALUES (@path)", connection);
                    //Add file path parameter to sql command (prevents sql injection)
                    command.Parameters.AddWithValue("@path", filePath);
                    command.ExecuteNonQuery();

                    MaintainTenEntries(connection);//Ensures only the latest 10 entries are kept
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void MaintainTenEntries(SQLiteConnection connection)
        {
            // First, count the number of entries
            var countCommand = new SQLiteCommand("SELECT COUNT(*) FROM FilePaths", connection);
            long count = (long)countCommand.ExecuteScalar();

            // If more than 10 entries, delete the oldest ones
            if (count > 10)
            {
                var deleteCommand = new SQLiteCommand("DELETE FROM FilePaths WHERE Id IN (SELECT Id FROM FilePaths ORDER BY Timestamp ASC LIMIT (SELECT COUNT(*) - 10 FROM FilePaths))", connection);
                deleteCommand.ExecuteNonQuery();
            }
        }

        public List<string> GetFilePaths()
        {
            //List to store file paths retrieved from database
            List<string> filePaths = new List<string>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                //sql command to select all paths in FilePaths table and order them by timestamp
                var command = new SQLiteCommand("SELECT Path FROM FilePaths ORDER BY Timestamp DESC", connection);

                using (var reader = command.ExecuteReader())//uses reader to fetch results of command
                {
                    while (reader.Read())//Iterate through rows of table and add to filePaths list
                    {
                        filePaths.Add(reader["Path"].ToString());
                    }
                }
            }

            return filePaths;
        }
    }
}
