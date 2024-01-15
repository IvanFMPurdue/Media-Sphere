using NUnit.Framework;
using Media_Sphere;

[TestFixture]
class UnitTest2
{
    [Test]
    public void AddRecentFile_Test()
    {
        // Arrange
        string filePath = "C:\\Example\\TestFile.txt"; // Provide a sample file path
        SQLiteSchema sqliteSchema = new SQLiteSchema();

        // Act
        sqliteSchema.AddRecentFile(filePath);

        // Assert
        string[] recentFiles = sqliteSchema.GetRecentFiles();
        Assert.That(recentFiles, Is.Not.Null);
        Assert.That(recentFiles, Contains.Item(filePath));
        // Add more assertions based on your method's behavior
    }

    [Test]
    public void GetRecentFiles_Test()
    {
        // Arrange
        SQLiteSchema sqliteSchema = new SQLiteSchema();

        // Act
        string[] recentFiles = sqliteSchema.GetRecentFiles();

        // Assert
        Assert.That(recentFiles, Is.Not.Null);
        // Add more assertions based on your method's behavior
    }
}
