using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileNameFixer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExecuteFileNameFixer(object sender, RoutedEventArgs e)
        {
            string folderPath = PathTextBox.Text.Trim();

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Invalid folder path!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int renamedCount = 0;
                var files = Directory.GetFiles(folderPath);

                foreach (var filePath in files)
                {
                    string? directory = Path.GetDirectoryName(filePath);
                    string originalFileName = Path.GetFileName(filePath);
                    string newFileName = ConvertToSnakeCaseToLower(originalFileName);

                    if (directory != null)
                    {
                        if (newFileName != originalFileName)
                        {
                            string newFilePath = Path.Combine(directory, newFileName);

                            // Handle potential name conflicts
                            if (File.Exists(newFilePath))
                            {
                                MessageBox.Show($"Skipped '{originalFileName}' - target name already exists",
                                              "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                continue;
                            }
                            
                            File.Move(filePath, newFilePath);
                            renamedCount++;
                        }
                    }
                }

                MessageBox.Show($"Successfully renamed {renamedCount} files!",
                              "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ConvertToSnakeCaseToLower(string input)
        {
            // Replace spaces with underscores and make lowercase
            return input.Trim()
                .Replace(" ", "_")
                .Replace("-", "_")
                .ToLowerInvariant();
        }

        private void ChooseFolder(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Folder"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                PathTextBox.Text = dialog.FileName;
            }
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Folder"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                PathTextBox.Text = dialog.FileName;
            }
        }
    }
}