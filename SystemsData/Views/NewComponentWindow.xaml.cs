using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SystemsData.Models;

namespace SystemsData.Views
{
    public partial class NewComponentWindow : Window
    {
        public BaseComponent? NewComponent { get; set; }
        private string? _selectedFilePath;

        public NewComponentWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "Все файлы (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                _selectedFilePath = dlg.FileName;
                txtFileAttachment.Text = _selectedFilePath;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var type = txtType.Text.Trim();
            var name = txtName.Text.Trim();
            var details = txtDetails.Text.Trim();
            var status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "проверено";
            var replacementInfo = txtReplacementInfo.Text.Trim();

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите тип и название компонента.");
                return;
            }

            byte[]? fileContent = null;
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                try
                {
                    fileContent = File.ReadAllBytes(_selectedFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
                    return;
                }
            }

            // Устанавливаем поля CreatedBy и UpdatedBy из текущего пользователя (App.CurrentEmployeeId)
            NewComponent = new BaseComponent
            {
                Type = type,
                Name = name,
                Details = details,
                Status = status,
                ReplacementInfo = replacementInfo,
                FileAttachment = fileContent,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = App.CurrentEmployeeId,
                UpdatedBy = App.CurrentEmployeeId
            };

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
