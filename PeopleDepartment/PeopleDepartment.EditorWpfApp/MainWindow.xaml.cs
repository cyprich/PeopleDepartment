using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly PersonCollection _personCollection = new();
        private DepartmentReport[]? _reports;
        private bool _wasCollectionModified = false;

        private bool _editEnabled;
        private bool _removeEnabled;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            EditEnabled = false;
            RemoveEnabled = false;
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
                    HandleSave(sender, e);
                }
                else if (response == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            _personCollection.Clear();
        }

        private void HandleOpen(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                _personCollection.LoadFromCsv(new FileInfo(filename));
                _reports = _personCollection.GenerateDepartmentReports();
                MainDataGrid.ItemsSource = _personCollection;
            }
        }

        private void HandleSave(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                _personCollection.SaveToCsv(new FileInfo(fileName));
            }
        }

        private void HandleView(object sender, RoutedEventArgs e)
        {
            var viewer = new PeopleDepartment.ViewerWpfApp.MainWindow();
            viewer.ShowDialog();
        }

        private void HandleAdd(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                _personCollection.Add(new Person(
                    addWindow.FirstName,
                    addWindow.LastName, 
                    addWindow.DisplayName, 
                    addWindow.Position,
                    addWindow.Email, 
                    addWindow.Department
                ));
            }
        }
        private void HandleEdit(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWindow();
            addWindow.ShowDialog();
            // TODO
        }

        private void HandleRemove(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedCells.Count < 1)
            {
                MessageBox.Show("There were no people selected to remove", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to remove? This action can't be undone",
                "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                var selectedItems = MainDataGrid.SelectedItems.Cast<Person>().ToList();
                foreach (var person in selectedItems)
                {
                    _personCollection.Remove(person);
                }
            }
        }

        private void HandleExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainDataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            EditEnabled = MainDataGrid.SelectedItems.Count == 1;
            RemoveEnabled = MainDataGrid.SelectedItems.Count >= 1;
        }

        // https://learn.microsoft.com/en-au/answers/questions/1857029/how-to-dynamically-enable-a-button-in-wpf-forms-wi
        public bool EditEnabled
        {
            get => _editEnabled;
            set { _editEnabled = value; OnPropertyChanged("EditEnabled"); }
        } 
        public bool RemoveEnabled
        {
            get => _removeEnabled;
            set { _removeEnabled = value; OnPropertyChanged("RemoveEnabled"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}