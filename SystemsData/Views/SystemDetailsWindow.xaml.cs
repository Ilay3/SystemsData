using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using SystemsData.Data;
using SystemsData.Models;
using SystemsData.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SystemsData.Views
{
    public partial class SystemDetailsWindow : Window
    {
        public SystemDetailsViewModel ViewModel { get; set; }

        public SystemDetailsWindow(SystemEntity system)
        {
            InitializeComponent();
            var context = App.AppHost.Services.GetRequiredService<ApplicationDbContext>();

            // Загружаем связанные базовые компоненты и их комплектующие
            context.Entry(system).Collection(s => s.BaseComponents).Query()
                   .Include(b => b.ComponentParts)
                   .Load();

            var empDict = context.Employees.AsNoTracking().ToDictionary(e => e.EmployeeId, e => e.FullName);

            // Формируем ViewModel
            ViewModel = new SystemDetailsViewModel
            {
                SystemId = system.SystemId,
                Name = system.Name,
                Description = system.Description,
                CreatedAt = system.CreatedAt,
                CreatedBy = system.CreatedBy,
                CreatedByName = empDict.ContainsKey(system.CreatedBy) ? empDict[system.CreatedBy] : system.CreatedBy.ToString(),
                // Если система имеет прикреплённый файл, установите его здесь
                FileAttachment = null,
                BaseComponents = new ObservableCollection<BaseComponentViewModel>(
                    system.BaseComponents.Select(b => new BaseComponentViewModel
                    {
                        ComponentId = b.ComponentId,
                        Type = b.Type,
                        Name = b.Name,
                        Status = b.Status,
                        Details = b.Details,
                        ReplacementInfo = b.ReplacementInfo,
                        FileAttachment = b.FileAttachment,
                        CreatedAt = b.CreatedAt,
                        CreatedBy = b.CreatedBy,
                        CreatedByName = empDict.ContainsKey(b.CreatedBy) ? empDict[b.CreatedBy] : b.CreatedBy.ToString(),
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

            DataContext = ViewModel;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BaseComponentView_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is BaseComponentViewModel compVM)
            {
                if (compVM.FileAttachment != null && compVM.FileAttachment.Length > 0)
                {
                    try
                    {
                        // Создаем временный файл с расширением .tmp (или нужным, если известно)
                        var tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                        System.IO.File.WriteAllBytes(tempFile, compVM.FileAttachment);
                        // Открываем файл через стандартное приложение
                        Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при открытии файла: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Файл не прикреплён.");
                }
            }
        }


        private void btnViewFile_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FileAttachment != null)
            {
                try
                {
                    var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                    File.WriteAllBytes(tempFile, ViewModel.FileAttachment);
                    Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии файла: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Файл не прикреплён.");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class SystemDetailsViewModel
    {
        public int SystemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public byte[]? FileAttachment { get; set; }
        public ObservableCollection<BaseComponentViewModel> BaseComponents { get; set; } = new ObservableCollection<BaseComponentViewModel>();
    }
}
