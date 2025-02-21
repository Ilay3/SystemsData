using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SystemsData.Data;
using SystemsData.Models;
using SystemsData.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace SystemsData.Views
{
    public partial class SystemsListGridView : UserControl
    {
        private readonly ApplicationDbContext _context;
        public ObservableCollection<SystemEntityViewModel> Systems { get; set; } = new ObservableCollection<SystemEntityViewModel>();

        public SystemsListGridView(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            DataContext = this;
            LoadSystems();
        }

        public SystemsListGridView() : this(App.AppHost.Services.GetRequiredService<ApplicationDbContext>())
        {
        }

        private void LoadSystems()
        {
            // Загружаем системы с компонентами и дополнительными комплектующими
            var systems = _context.SystemEntities
                .Include(s => s.BaseComponents)
                    .ThenInclude(b => b.ComponentParts)
                .AsNoTracking()
                .ToList();

            // Создаем словарь сотрудников для поиска FullName по CreatedBy
            var empDict = _context.Employees.AsNoTracking().ToDictionary(e => e.EmployeeId, e => e.FullName);

            Systems.Clear();
            foreach (var sys in systems)
            {
                var sysVM = new SystemEntityViewModel
                {
                    SystemId = sys.SystemId,
                    Name = sys.Name,
                    Description = sys.Description,
                    CreatedAt = sys.CreatedAt,
                    CreatedBy = sys.CreatedBy,
                    CreatedByName = empDict.ContainsKey(sys.CreatedBy) ? empDict[sys.CreatedBy] : sys.CreatedBy.ToString(),
                    BaseComponents = new ObservableCollection<BaseComponentViewModel>(
                        sys.BaseComponents.Select(b => new BaseComponentViewModel
                        {
                            ComponentId = b.ComponentId,
                            Type = b.Type,
                            Name = b.Name,
                            Status = b.Status,
                            Details = b.Details,
                            ComponentParts = new ObservableCollection<ComponentPartViewModel>(
                                b.ComponentParts.Select(p => new ComponentPartViewModel
                                {
                                    PartId = p.PartId,
                                    PartType = p.PartType,
                                    PartName = p.PartName,
                                    AdditionalDetails = p.AdditionalDetails
                                })
                            )
                        })
                    )
                };
                Systems.Add(sysVM);
            }
        }

        private void MenuItem_Details_Click(object sender, RoutedEventArgs e)
        {
            if (dgSystems.SelectedItem is SystemEntityViewModel selectedSystem)
            {
                // Загружаем систему из базы для полной информации
                var systemEntity = _context.SystemEntities
                    .Include(s => s.BaseComponents)
                        .ThenInclude(b => b.ComponentParts)
                    .AsNoTracking()
                    .FirstOrDefault(s => s.SystemId == selectedSystem.SystemId);
                if (systemEntity != null)
                {
                    var detailsWindow = new SystemDetailsWindow(systemEntity);
                    detailsWindow.ShowDialog();
                }
            }
        }

        private void dgSystems_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Можно добавить дополнительную обработку, если необходимо
        }
    }
}
