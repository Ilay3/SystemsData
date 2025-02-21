using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using SystemsData.Models;
using System.Diagnostics;
using System.Windows.Controls;

namespace SystemsData.Views
{
    public partial class EditComponentWindow : Window
    {
        private BaseComponent _component;
        private string _selectedFilePath;

        public EditComponentWindow(BaseComponent component)
        {
            InitializeComponent();
            _component = component;
            LoadComponentData();
        }

        private void LoadComponentData()
        {
            txtType.Text = _component.Type;
            txtName.Text = _component.Name;
            txtDetails.Text = _component.Details;
            txtReplacementInfo.Text = _component.ReplacementInfo;
            txtCreatedAt.Text = _component.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
            txtFileAttachment.Text = _component.FileAttachment != null && _component.FileAttachment.Length > 0 ? "Файл загружен" : "";
            foreach (var item in cmbStatusEdit.Items)
            {
                if (item is ComboBoxItem cbi && cbi.Content.ToString() == _component.Status)
                {
                    cmbStatusEdit.SelectedItem = cbi;
                    break;
                }
            }
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Title = "Выберите файл", Filter = "Все файлы (*.*)|*.*" };
            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                txtFileAttachment.Text = _selectedFilePath;
            }
        }

        private void btnViewFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                try
                {
                    var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(_selectedFilePath)}");
                    File.WriteAllBytes(tempFile, _component.FileAttachment);
                    Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + ex.Message);
                }
            }
            else if (_component.FileAttachment != null && _component.FileAttachment.Length > 0)
            {
                try
                {
                    var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                    File.WriteAllBytes(tempFile, _component.FileAttachment);
                    Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Файл не прикреплен.");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _component.Type = txtType.Text;
            _component.Name = txtName.Text;
            _component.Details = txtDetails.Text;
            _component.ReplacementInfo = txtReplacementInfo.Text;
            var status = (cmbStatusEdit.SelectedItem as ComboBoxItem)?.Content.ToString();
            _component.Status = status;
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                try
                {
                    _component.FileAttachment = File.ReadAllBytes(_selectedFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
                }
            }
            // Здесь UpdatedBy и UpdatedAt будут обновлены вызывающим кодом
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
