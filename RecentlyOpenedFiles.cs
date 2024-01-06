using SQLite;
using SQLite.Net.Attributes;
using System;

internal class RecentlyOpenedFiles
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FileName { get; set; }

    public DateTime OpenedDate { get; set; }

    // Additional properties or methods if needed

    public RecentlyOpenedFiles()
    {
        // Default constructor required for SQLite
    }

    public RecentlyOpenedFiles(string fileName, DateTime openedDate)
    {
        FileName = fileName;
        OpenedDate = openedDate;
    }
}
