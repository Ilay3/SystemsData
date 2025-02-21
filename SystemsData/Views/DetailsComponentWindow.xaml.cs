using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using SystemsData.Models;
using Microsoft.Extensions.DependencyInjection;
using SystemsData.Data;
using Microsoft.EntityFrameworkCore;
using SystemsData.ViewModels;

namespace SystemsData.Views
{
    public partial class DetailsComponentWindow : Window
    {
        public BaseComponent Component { get; set; }
        public List<ComponentPart> AdditionalParts { get; set; }

        public DetailsComponentWindow(BaseComponent component, List<ComponentPart> additionalParts)
        {
            InitializeComponent();
            Component = component;
            AdditionalParts = additionalParts;

            // Получаем имя сотрудника, создавшего запись
            var context = App.AppHost.Services.GetRequiredService<ApplicationDbContext>();
            var emp = context.Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeId == component.CreatedBy);
            string createdByName = emp != null ? emp.FullName : component.CreatedBy.ToString();

            DataContext = new DetailsComponentViewModel
            {
                ComponentId = component.ComponentId,
                Type = component.Type,
                Name = component.Name,
                Details = component.Details,
                Status = component.Status,
                ReplacementInfo = component.ReplacementInfo,
                CreatedAt = component.CreatedAt,
                CreatedByName = createdByName,
                FileAttachment = component.FileAttachment,
                FileAttachmentLength = component.FileAttachment != null ? $"{component.FileAttachment.Length} байт" : "Нет файла"
            };

            dgAdditionalParts.ItemsSource = AdditionalParts;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnViewFile_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as DetailsComponentViewModel;
            if (vm.FileAttachment != null)
            {
                try
                {
                    var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
                    File.WriteAllBytes(tempFile, vm.FileAttachment);
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

    
}
