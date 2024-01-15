using NUnit.Framework;
using Media_Sphere;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

[TestFixture]
class UnitTest3
{
    [Test]
    public void FileOpenButton_Click_Test()
    {
        // Arrange
        MainWindow mainWindowInstance = new MainWindow();
        string testFilePath = Path.Combine("test_data", "example.txt"); // Provide a sample file path for testing

        // Act
        InvokePrivateMethod(mainWindowInstance, "FileOpenButton_Click", null, null);

        // Simulate file selection in the dialog
        InvokePrivateMethod(mainWindowInstance, "RecentFilesButton_Click", null, null);

        // Assert
        string[] recentFiles = GetPrivateField<string[]>(mainWindowInstance, "recentFiles"); // Adjust the field name accordingly
        Assert.That(recentFiles, Is.Not.Null);
        Assert.That(recentFiles, Contains.Item(testFilePath));
        // Add more assertions based on your method's behavior
    }

    private void InvokePrivateMethod(object obj, string methodName, params object[] parameters)
    {
        MethodInfo methodInfo = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            methodInfo.Invoke(obj, parameters);
        }
    }

    private T GetPrivateField<T>(object obj, string fieldName)
    {
        FieldInfo fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            return (T)fieldInfo.GetValue(obj);
        }
        return default(T);
    }
}
