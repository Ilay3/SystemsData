using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SystemsData.Data;
using SystemsData.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace SystemsData.Views
{
    public partial class SelectComponentWindow : Window
    {
        private readonly ApplicationDbContext _context;
        public List<BaseComponent> SelectedComponents { get; private set; } = new List<BaseComponent>();

        public SelectComponentWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadComponents();
        }

        public SelectComponentWindow() : this(App.AppHost.Services.GetRequiredService<ApplicationDbContext>())
        {
        }

        private void LoadComponents()
        {
            // Загружаем компоненты, которые еще не привязаны к системе
            var comps = _context.Base_Components.AsNoTracking().Where(c => c.SystemId == null).ToList();
            dgAvailableComponents.ItemsSource = comps;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectedComponents = dgAvailableComponents.SelectedItems.Cast<BaseComponent>().ToList();
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
