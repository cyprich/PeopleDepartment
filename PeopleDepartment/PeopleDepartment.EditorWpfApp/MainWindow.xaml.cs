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

namespace PeopleDepartment.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _wasCollectionModified = false;
        private PersonCollection _personCollection = new();
        private DepartmentReport[]? _reports;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HandleAbout(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void HandleNew(object sender, RoutedEventArgs e)
        {
            if (_wasCollectionModified)
            {
                var response = MessageBox.Show(
                    "The collection has been modified. Do you want to save it?", 
                    "Save collection", 
                    MessageBoxButton.YesNoCancel);

                if (response == MessageBoxResult.Yes)
                {
                    // TODO ulozit 
                } else if (response == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            // TODO vymazat aktualnu collection
        }

        private void HandleOpen(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                _personCollection.LoadFromCSV(new FileInfo(filename));
                _reports = _personCollection.GenerateDepartmentReports();
                MainDataGrid.ItemsSource = _personCollection;
            }
        }

        private void HandleSave(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleView(object sender, RoutedEventArgs e)
        {
            var viewer = new PeopleDepartment.ViewerWpfApp.MainWindow();
            viewer.ShowDialog();
        }

        private void HandleAdd(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddWindow();
            addWindow.ShowDialog();
        }
        private void HandleEdit(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleRemove(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleExit(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}