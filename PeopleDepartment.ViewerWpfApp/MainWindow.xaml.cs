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
        private readonly PersonCollection _personCollection = [];
        private DepartmentReport[]? _reports;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(PersonCollection personCollection)
        {
            InitializeComponent();

            _personCollection = personCollection;
            OpenButton.IsEnabled = false;
            GenerateReports();
        }

        private void GenerateReports()
        {
            _reports = _personCollection.GenerateDepartmentReports();
            DepartmentComboBox.ItemsSource = _reports.Select(r => r.Department);
            DepartmentComboBox.SelectedIndex = 0;
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                _personCollection.LoadFromCsv(new FileInfo(filename));
                GenerateReports();
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