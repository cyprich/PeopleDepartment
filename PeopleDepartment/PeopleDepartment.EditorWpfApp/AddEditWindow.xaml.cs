using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PeopleDepartment.EditorWpfApp
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string? TitleBefore { get; set; } = "";
        public string? TitleAfter { get; set; } = "";
        public string? Position { get; set; } = "";
        public string Email { get; set; } = "";
        public string Department { get; set; } = "";


        public AddEditWindow()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            OnTextChanged(sender, null);

            Position = PositionTextBox.Text;
            Email = EmailTextBox.Text;
            Department = DepartmentTextBox.Text;

            if (ValidateParameters())
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("All required fields must be filled", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FirstName = FirstNameTextBox.Text.Trim();
            LastName = LastNameTextBox.Text.Trim();
            TitleBefore = TitleBeforeTextBox.Text.Trim();
            TitleAfter = TitleAfterTextBox.Text.Trim();

            DisplayName = "";
            if (TitleBefore != "")
            {
                DisplayName += $"{TitleBefore} ";
            }

            DisplayName += $"{FirstName} {LastName}";

            if (TitleBefore != "")
            {
                DisplayName += $", {TitleAfter}";
            }
        }

        private bool ValidateParameters()
        {
            var required = new[] { FirstName, LastName, Email, Department };

            foreach (var i in required)
            {
                if (i == "") { return false; }
            }

            return true;
        }
    }
}
