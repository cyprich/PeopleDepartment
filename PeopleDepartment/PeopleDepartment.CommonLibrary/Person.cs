using System.ComponentModel;

namespace PeopleDepartment.CommonLibrary
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string? TitleBefore { get; }
        public string? TitleAfter { get; }
        public string? Position { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }

        // TODO
        public event PropertyChangedEventHandler? PropertyChanged;

        public Person(string firstName, string lastName, string displayName, string? position, string email, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Position = position;
            Email = email;
            Department = department;

            TitleAfter = DisplayName.Contains(",") ? displayName.Split(",")[^1][1..] : "";

            // TODO TitleBefore
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

        public string ToCSV(string delimiter = ";")
        {
            return string.Join(delimiter, [FirstName, LastName, DisplayName, Position, Email, Department]);
        }
    }
}
