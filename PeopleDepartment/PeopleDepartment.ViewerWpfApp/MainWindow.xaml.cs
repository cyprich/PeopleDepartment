using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PeopleDepartment.CommonLibrary;

namespace PeopleDepartment.ViewerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PersonCollection _personCollection = new();
        private DepartmentReport[]? _reports;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                _personCollection.LoadFromCSV(new FileInfo(filename));
                _reports = _personCollection.GenerateDepartmentReports();
                DepartmentComboBox.ItemsSource = _reports.Select(r => r.Department);
                DepartmentComboBox.SelectedIndex = 0;
            }
        }

        private void DepartmentComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = DepartmentComboBox.SelectedIndex;

            if (_reports != null && _reports.Length >= index)
            {
                HeadTextBox.Text = _reports[index].Head?.ToString();
                DeputyTextBox.Text = _reports[index].Deputy?.ToString();
                SecretaryTextBox.Text = _reports[index].Secretary?.ToString();

                EmployeeDataGrid.ItemsSource = _personCollection
                    .Where(p => 
                        (p.Department == DepartmentComboBox.SelectedItem as string &&
                         p.Position != "doktorand"));
                PhDDataGrid.ItemsSource = _personCollection
                    .Where(p => 
                        (p.Department == DepartmentComboBox.SelectedItem as string &&
                         p.Position == "doktorand"));
            }
        }
    }
}