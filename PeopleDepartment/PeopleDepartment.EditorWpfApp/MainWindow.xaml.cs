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
        private bool _wasCollectionModified = false;

        private bool _editEnabled;
        private bool _removeEnabled;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            EditEnabled = false;
            RemoveEnabled = false;

            MainDataGrid.ItemsSource = _personCollection;
        }


        private void HandleAbout(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void HandleNew(object sender, RoutedEventArgs e)
        {
            if (HandleModifiedCollection())
            {
                _personCollection.Clear();
            }
        }

        private void HandleOpen(object sender, RoutedEventArgs e)
        {
            if (HandleModifiedCollection())
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true)
                {
                    var filename = openFileDialog.FileName;
                    _personCollection.LoadFromCsv(new FileInfo(filename));
                }
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
            var viewer = new PeopleDepartment.ViewerWpfApp.MainWindow(_personCollection);
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

            _wasCollectionModified = true;
        }

        private void HandleEdit(object sender, RoutedEventArgs e)
        {
            int count = MainDataGrid.SelectedItems.Count;
            if (count < 1)
            {
                MessageBox.Show("You did not provide person to edit", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (count > 1)
            {
                MessageBox.Show("You can edit only one person at one time", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Person person = MainDataGrid.SelectedItems.Cast<Person>().First();

            var editWindow = new AddEditWindow(person);
            if (editWindow.ShowDialog() == true)
            {
                person.FirstName = editWindow.FirstName;
                person.LastName = editWindow.LastName;
                person.DisplayName = editWindow.DisplayName;
                person.TitleBefore = editWindow.TitleBefore;
                person.TitleAfter = editWindow.TitleAfter;
                person.Position = editWindow.Position;
                person.Email = editWindow.Email;
                person.Department = editWindow.Department;
            }

            _wasCollectionModified = true;
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

            _wasCollectionModified = true;
        }

        private void HandleExit(object sender, RoutedEventArgs e)
        {
            if (HandleModifiedCollection())
            {
                Close();
            }
        }

        // return - true if run correctly - user did not press "Cancel" - thus can be deleted
        private bool HandleModifiedCollection()
        {
            if (_wasCollectionModified)
            {
                var result = MessageBox.Show(
                    "The collection has been modified. Do you want to save it?",
                    "Save collection",
                    MessageBoxButton.YesNoCancel, 
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    HandleSave(this, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }

                _wasCollectionModified = false;
            }

            return true;
        }

        private void MainDataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int count = MainDataGrid.SelectedItems.Count;
            EditEnabled = count == 1;
            RemoveEnabled = count >= 1;
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