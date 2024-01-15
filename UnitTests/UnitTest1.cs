using NUnit.Framework;
using Media_Sphere;

[TestFixture]
class UnitTest1
{
    [Test]
    public void RecentFilesButton_Click_Test()
    {
        // Arrange
        MainWindow mainWindowInstance = new MainWindow();

        // Act
        mainWindowInstance.RecentFilesButton_Click(null, null); // Simulate button click

        // Assert
        Assert.That(mainWindowInstance.RecentFilesMenu, Is.Not.Null);
        // Add more assertions based on your method's behavior
    }
}
