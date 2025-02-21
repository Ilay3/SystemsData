using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SystemsData.Data;
using SystemsData.Models;
using Microsoft.Extensions.DependencyInjection;
using SystemsData.ViewModels;

namespace SystemsData.Views
{
    public partial class AcceptanceView : UserControl
    {
        private readonly ApplicationDbContext _context;

        // Коллекция для хранения добавленных комплектующих, отображаемых в DataGrid
        public ObservableCollection<ComponentPartViewModel> AdditionalParts { get; set; }

        // Хранит путь к выбранному файлу (для отображения в TextBox)
        private string _selectedFilePath;

        // Основной конструктор – получает контекст через DI
        public AcceptanceView(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            AdditionalParts = new ObservableCollection<ComponentPartViewModel>();
            dgParts.ItemsSource = AdditionalParts;
        }

        // Публичный конструктор без параметров для использования в XAML
        public AcceptanceView() : this(App.AppHost.Services.GetRequiredService<ApplicationDbContext>())
        {
        }

        private void btnAddPart_Click(object sender, RoutedEventArgs e)
        {
            // Открываем модальное окно для ввода данных комплектующего
            var addPartWindow = new AddPartWindow();
            bool? result = addPartWindow.ShowDialog();
            if (result == true && addPartWindow.NewPart != null)
            {
                AdditionalParts.Add(addPartWindow.NewPart);
            }
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалог выбора файла
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "Все файлы (*.*)|*.*"  
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                txtFileAttachment.Text = _selectedFilePath;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var componentType = (cmbComponentType.SelectedItem as ComboBoxItem)?.Content.ToString();
            var componentName = txtComponentName.Text;
            var details = txtDetails.Text;
            // Получаем выбранный статус из ComboBox
            var status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            var replacementInfo = txtReplacementInfo.Text;
            var fileAttachment = txtFileAttachment.Text;

            if (string.IsNullOrWhiteSpace(componentType) || string.IsNullOrWhiteSpace(componentName))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Тип и Название.");
                return;
            }

            // Читаем содержимое файла, если выбран
            byte[] fileContent = null;
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

            // Создаем запись в таблице Base_Components
            var baseComponent = new BaseComponent
            {
                Type = componentType,
                Name = componentName,
                Details = details,
                Status = status, // сохранится выбранное значение
                ReplacementInfo = replacementInfo,
                FileAttachment = fileContent,
                CreatedBy = App.CurrentEmployeeId,
                UpdatedBy = App.CurrentEmployeeId
            };

            _context.Base_Components.Add(baseComponent);
            _context.SaveChanges(); // Сохраняем для получения сгенерированного ComponentId

            // Сохраняем дополнительные комплектующие, если они есть
            foreach (var part in AdditionalParts)
            {
                var componentPart = new ComponentPart
                {
                    ParentComponentId = baseComponent.ComponentId,
                    PartType = part.PartType,
                    PartNumber = part.PartNumber,
                    PartName = part.PartName,
                    AdditionalDetails = part.AdditionalDetails
                };
                _context.Component_Parts.Add(componentPart);
            }
            _context.SaveChanges();

            MessageBox.Show("Приемка успешно сохранена.");

            // Очистка формы
            cmbComponentType.SelectedIndex = 0;
            txtComponentName.Clear();
            txtDetails.Clear();
            cmbStatus.SelectedIndex = 0;
            txtReplacementInfo.Clear();
            txtFileAttachment.Clear();
            _selectedFilePath = null;
            AdditionalParts.Clear();
        }

    }

}
