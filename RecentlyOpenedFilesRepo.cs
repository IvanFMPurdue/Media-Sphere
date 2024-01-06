using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

internal class RecentlyOpenedFilesRepo : IDisposable
{
    private readonly SQLiteConnection _connection;

    public RecentlyOpenedFilesRepo(string dbPath)
    {
        _connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
        _connection.Open();  // Make sure the connection is opened
        _connection.CreateTable<RecentlyOpenedFiles>();
    }

    public void AddRecentlyOpenedFile(RecentlyOpenedFiles file)
    {
        _connection.Insert(file);
    }

    public List<RecentlyOpenedFiles> GetRecentlyOpenedFiles()
    {
        return _connection.Table<RecentlyOpenedFiles>().OrderByDescending(f => f.OpenedDate).ToList();
    }

    // Additional CRUD operations as needed

    public void Dispose()
    {
        _connection.Dispose();
    }
}
