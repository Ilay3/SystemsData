using System.Windows;
using SystemsData.ViewModels;

namespace SystemsData.Views
{
    public partial class AddPartWindow : Window
    {
        public ComponentPartViewModel NewPart { get; set; }

        public AddPartWindow()
        {
            InitializeComponent();
        }

        private void btnSavePart_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные и создаем новый ViewModel для комплектующего
            NewPart = new ComponentPartViewModel
            {
                PartType = txtPartType.Text,
                PartNumber = txtPartNumber.Text,
                PartName = txtPartName.Text,
                AdditionalDetails = txtAdditionalDetails.Text
            };
            DialogResult = true;
            Close();
        }

        private void btnCancelPart_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
