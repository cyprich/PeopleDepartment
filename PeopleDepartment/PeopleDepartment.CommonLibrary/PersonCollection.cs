using System.Collections.Specialized;

namespace PeopleDepartment.CommonLibrary
{
    public class PersonCollection
    {
        public int Count { get; }
        public bool IsReadOnly { get; }
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        private readonly List<Person> _people = [];

        public void Add(Person person) { _people.Add(person); }

        public bool Remove(Person person) { return _people.Remove(person); }

        public IEnumerator<Person> GetEnumerator() { return _people.GetEnumerator(); }

        public void LoadFromCSV(FileInfo csvFile)
        {
            var lines = File.ReadAllLines(csvFile.Name);
            lines = lines[1..];  // skip first line with field names

            foreach (var line in lines)
            {
                var s = line.Split(";");
                Add(new Person(s[0], s[1], s[2], s[3], s[4], s[5]));
            }
        }

        //public void saveToCSV(FileInfo csvFile) { }

        public DepartmentReport[] GenerateDepartmentReports()
        {
            List<DepartmentReport> result = [];
            List<string> doneDepartments = [];

            while (true)
            {
                string currentDepartmentName = "";
                Person? head = null;
                Person? deputy = null; 
                Person? secretary = null;
                List<Person> employees = []; 
                List<Person> phDStudents = [];
                int profs = 0;
                int docs = 0;

                foreach (var p in _people)
                {
                    if (currentDepartmentName == "" && !doneDepartments.Contains(p.Department))
                    {
                        currentDepartmentName = p.Department;
                        doneDepartments.Add(p.Department);
                    }

                    if (currentDepartmentName != "" && p.Department == currentDepartmentName)
                    {
                        switch (p.Position)
                        {
                            case "dekan":
                            case "vedúci":
                                head = p;
                                break;
                            case "zástupca vedúceho":
                                deputy = p;
                                break;
                            case "sekretárka":
                                secretary = p;
                                break;
                        }

                        if (p.Position == "doktorand")
                        {
                            phDStudents.Add(p);
                        }
                        else
                        {
                            employees.Add(p);
                        }

                        if (p.TitleBefore != null)
                        {

                            if (p.TitleBefore.Contains("prof."))
                            {
                                profs++;
                            }
                            else if (p.TitleBefore.Contains("doc. "))
                            {
                                docs++;
                            }
                        }
                    }
                }

                if (currentDepartmentName == "") { break; }

                result.Add(new DepartmentReport(
                    currentDepartmentName,
                    head,
                    deputy,
                    secretary,
                    profs,
                    docs,
                    employees,
                    phDStudents
                ));
            }

            return result.ToArray();
        }

        public void Clear() { _people.Clear(); }

        public bool Contains(Person person) { return _people.Contains(person); }

        //public void CopyTo(Person[] array, int arrayIndex) { }

        public override string ToString()
        {
            string result = "";

            foreach (var p in _people)
            {
                result += $"{p.ToFormattedString()}\n";
            }

            return result;
        }
    }
}
