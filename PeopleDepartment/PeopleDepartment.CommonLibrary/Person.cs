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
        public event PropertyChangedEventHandler? PropertyChanged;

        public Person(string firstName, string lastName, string displayName, string? position, string email, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Position = position;
            Email = email;
            Department = department;
            TitleBefore = "";
            TitleAfter = "";
            // TODO TitleBefore and TitleAfter from displayName
        }

        public string ToFormattedString()
        {
            return $"{DisplayName,-40}{Email}";
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
