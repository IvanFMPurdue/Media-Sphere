using System;


namespace Media_Sphere


public class RecentFiles
{
    public RecentFiles()
    {
        recentFiles = LoadRecentFiles();

    public void SaveRecentFiles(string filePath)
        {
            const int maxRecentFiles = 10;

            if (!recentFiles.Contains(filePath))
            {
                recentFiles.Insert(0, filePath);

                if (recentFiles.Count > maxRecentFiles)
                {
                    recentFiles.RemoveAt(maxRecentFiles);
                }

                string saveFilePath = openFileDialog.FileName;

                try
                {
                    File.WriteAllLines(saveFilePath, recentFiles);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving recent files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
	}
}
