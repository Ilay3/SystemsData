using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using SystemsData.Data;
using SystemsData.Models;

namespace SystemsData.Views
{
    public partial class AuthorizationWindow : Window
    {
        private readonly ApplicationDbContext _context;

        // Получаем ApplicationDbContext через DI
        public AuthorizationWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            // Загружаем список сотрудников из базы данных
            var employees = _context.Employees.ToList();
            cmbEmployees.ItemsSource = employees;
            if (employees.Any())
                cmbEmployees.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmployees.SelectedItem is Employee selectedEmployee)
            {
                App.CurrentEmployeeId = selectedEmployee.EmployeeId;
                // Получаем главное окно через DI
                var mainWindow = App.AppHost.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя.", "Ошибка");
            }
        }

    }
}
