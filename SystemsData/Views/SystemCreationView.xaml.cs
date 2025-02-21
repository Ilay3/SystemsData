using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemsData.Data;
using SystemsData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SystemsData.Views
{
    public partial class SystemCreationView : UserControl
    {
        private readonly ApplicationDbContext _context;
        // Список выбранных компонентов для системы
        public ObservableCollection<BaseComponent> SelectedComponents { get; set; } = new ObservableCollection<BaseComponent>();

        public SystemCreationView(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            dgSelectedComponents.ItemsSource = SelectedComponents;
        }

        // Параметрless конструктор для использования в XAML
        public SystemCreationView() : this(App.AppHost.Services.GetRequiredService<ApplicationDbContext>())
        {
        }

        // Обработчик кнопки "Добавить существующий"
        private void btnAddExisting_Click(object sender, RoutedEventArgs e)
        {
            var selectWindow = new SelectComponentWindow();
            bool? result = selectWindow.ShowDialog();
            if (result == true)
            {
                foreach (var comp in selectWindow.SelectedComponents)
                {
                    if (!SelectedComponents.Any(c => c.ComponentId == comp.ComponentId))
                    {
                        SelectedComponents.Add(comp);
                    }
                }
            }
        }

        // Обработчик кнопки "Добавить новый"
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            var newCompWindow = new NewComponentWindow();
            bool? result = newCompWindow.ShowDialog();
            if (result == true && newCompWindow.NewComponent != null)
            {
                // Сохраняем новый компонент в таблицу Base_Components
                var newComponent = newCompWindow.NewComponent;
                newComponent.CreatedBy = App.CurrentEmployeeId;
                newComponent.UpdatedBy = App.CurrentEmployeeId;
                _context.Base_Components.Add(newComponent);
                _context.SaveChanges();
                SelectedComponents.Add(newComponent);
            }
        }

        // Контекстное меню: "Доп. информация" для выбранного компонента
        private void MenuItem_Details_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedComponents.SelectedItem is BaseComponent selectedComponent)
            {
                var additionalParts = _context.Component_Parts
                    .Where(cp => cp.ParentComponentId == selectedComponent.ComponentId)
                    .ToList();
                var detailsWindow = new DetailsComponentWindow(selectedComponent, additionalParts);
                detailsWindow.ShowDialog();
            }
        }

        // Сохранение системы
        private void btnSaveSystem_Click(object sender, RoutedEventArgs e)
        {
            var systemName = txtSystemName.Text.Trim();
            if (string.IsNullOrEmpty(systemName))
            {
                MessageBox.Show("Введите название системы.");
                return;
            }

            var systemEntity = new SystemEntity
            {
                Name = systemName,
                Description = txtSystemDescription.Text.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = App.CurrentEmployeeId,
                UpdatedBy = App.CurrentEmployeeId
            };

            _context.SystemEntities.Add(systemEntity);
            _context.SaveChanges();

            // Обновляем каждый выбранный компонент, устанавливая SystemId
            foreach (var comp in SelectedComponents)
            {
                var compToUpdate = _context.Base_Components.Find(comp.ComponentId);
                if (compToUpdate != null)
                {
                    compToUpdate.SystemId = systemEntity.SystemId;
                    _context.Entry(compToUpdate).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

            MessageBox.Show("Система успешно создана.");
            // Очистка формы
            txtSystemName.Clear();
            txtSystemDescription.Clear();
            SelectedComponents.Clear();
        }
    }
}
