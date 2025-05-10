using System.ComponentModel;

namespace PeopleDepartment.CommonLibrary
{
    public class Person : INotifyPropertyChanged
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private string? _titleBefore;
        public string? TitleBefore
        {
            get => _titleBefore;
            set
            {
                _titleBefore = value;
                OnPropertyChanged(nameof(TitleBefore));
            }
        }

        private string? _titleAfter;
        public string? TitleAfter
        {
            get => _titleAfter;
            set
            {
                _titleAfter = value;
                OnPropertyChanged(nameof(TitleAfter));
            }
        }

        private string? _position;
        public string? Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _department;
        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        public Person(string firstName, string lastName, string displayName, string? position, string email, string department)
        {
            _firstName = firstName;
            _lastName = lastName;
            _displayName = displayName;
            _position = position;
            _email = email;
            _department = department;

            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Position = position;
            Email = email;
            Department = department;

            // title after
            TitleAfter = DisplayName.Contains(',') ? displayName.Split(",")[^1][1..] : "";

            // title before
            var fields = displayName.Split(" ");
            if (TitleAfter != "")
            {
                fields = fields[..^(TitleAfter.Split(" ").Length)];  // remove titles after name
            }
            fields = fields[..^2];  // remove name and surname 
            TitleBefore = string.Join(" ", fields);
        }

        public string ToFormattedString()
        {
            return $"{DisplayName,-40}{Email}";
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public string ToCsv(string delimiter = ";")
        {
            return string.Join(delimiter, [FirstName, LastName, DisplayName, Position, Email, Department]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
