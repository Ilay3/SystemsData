using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SystemsData.Data;
using SystemsData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemsData.ViewModels;

namespace SystemsData.Views
{
    public partial class FaultyComponentsView : UserControl
    {
        private readonly ApplicationDbContext _context;
        public ObservableCollection<FaultyComponentViewModel> Components { get; set; }

        public FaultyComponentsView(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            Components = new ObservableCollection<FaultyComponentViewModel>();
            dgComponents.ItemsSource = Components;
            LoadComponents();
        }

        // Конструктор по умолчанию для XAML
        public FaultyComponentsView() : this(App.AppHost.Services.GetRequiredService<ApplicationDbContext>())
        {
        }

        private void LoadComponents()
        {
            var empDict = _context.Employees.AsNoTracking().ToDictionary(e => e.EmployeeId, e => e.FullName);
            var comps = _context.Base_Components.AsNoTracking().ToList();
            Components.Clear();
            foreach (var comp in comps)
            {
                Components.Add(new FaultyComponentViewModel
                {
                    ComponentId = comp.ComponentId,
                    Type = comp.Type,
                    Name = comp.Name,
                    Status = comp.Status,
                    CreatedAt = comp.CreatedAt,
                    CreatedByName = empDict.ContainsKey(comp.CreatedBy) ? empDict[comp.CreatedBy] : comp.CreatedBy.ToString()
                });
            }
        }

        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            var query = _context.Base_Components.AsQueryable();

            var selectedStatus = (cmbFilterStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedStatus != "Все")
                query = query.Where(c => c.Status == selectedStatus);

            var nameFilter = txtFilterName.Text;
            if (!string.IsNullOrWhiteSpace(nameFilter))
                query = query.Where(c => c.Name.Contains(nameFilter));

            if (dpDateFrom.SelectedDate.HasValue)
                query = query.Where(c => c.CreatedAt >= dpDateFrom.SelectedDate.Value.ToUniversalTime());
            if (dpDateTo.SelectedDate.HasValue)
                query = query.Where(c => c.CreatedAt < dpDateTo.SelectedDate.Value.AddDays(1).ToUniversalTime());


            var result = query.ToList();
            var empDict = _context.Employees.AsNoTracking().ToDictionary(e => e.EmployeeId, e => e.FullName);
            Components.Clear();
            foreach (var comp in result)
            {
                Components.Add(new FaultyComponentViewModel
                {
                    ComponentId = comp.ComponentId,
                    Type = comp.Type,
                    Name = comp.Name,
                    Status = comp.Status,
                    CreatedAt = comp.CreatedAt,
                    CreatedByName = empDict.ContainsKey(comp.CreatedBy) ? empDict[comp.CreatedBy] : comp.CreatedBy.ToString()
                });
            }
        }

        private void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgComponents.SelectedItem is FaultyComponentViewModel selectedVM)
            {
                
                var selectedComponent = _context.Base_Components.FirstOrDefault(c => c.ComponentId == selectedVM.ComponentId);
                if (selectedComponent != null)
                {
                    var editWindow = new EditComponentWindow(selectedComponent);
                    bool? result = editWindow.ShowDialog();
                    if (result == true)
                    {
                        _context.Entry(selectedComponent).State = EntityState.Modified;
                        selectedComponent.UpdatedBy = App.CurrentEmployeeId;
                        selectedComponent.UpdatedAt = DateTime.UtcNow;
                        _context.SaveChanges();
                        LoadComponents();
                    }
                }
            }
        }

        private void MenuItem_Details_Click(object sender, RoutedEventArgs e)
        {
            if (dgComponents.SelectedItem is FaultyComponentViewModel selectedVM)
            {
                var selectedComponent = _context.Base_Components.FirstOrDefault(c => c.ComponentId == selectedVM.ComponentId);
                if (selectedComponent != null)
                {
                    var additionalParts = _context.Component_Parts
                        .Where(cp => cp.ParentComponentId == selectedComponent.ComponentId)
                        .ToList();
                    var detailsWindow = new DetailsComponentWindow(selectedComponent, additionalParts);
                    detailsWindow.ShowDialog();
                }
            }
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            // Сброс фильтров на значения по умолчанию
            cmbFilterStatus.SelectedIndex = 0; // "Все"
            txtFilterName.Clear();
            dpDateFrom.SelectedDate = null;
            dpDateTo.SelectedDate = null;
            // Обновляем список без фильтров
            LoadComponents();
        }


        private void dgComponents_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            
        }
    }

    
}
