using System.Collections;
using System.Collections.Specialized;

namespace PeopleDepartment.CommonLibrary
{
    public class PersonCollection : ICollection<Person>, INotifyCollectionChanged
    {
        public int Count { get; }
        public bool IsReadOnly { get; }
        public event NotifyCollectionChangedEventHandler? CollectionChanged;  
        private readonly List<Person> _people = [];

        public void Add(Person person)
        {
            _people.Add(person);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, person));
        }

        public bool Remove(Person person)
        {
            int index = _people.IndexOf(person);
            if (index >= 0)
            {
                _people.RemoveAt(index);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, person, index));
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _people.Clear(); 
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(Person person) { return _people.Contains(person); }

        public override string ToString()
        {
            string result = "";

            foreach (var p in _people)
            {
                result += $"{p.ToFormattedString()}\n";
            }

            return result;
        }

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
                            else if (p.TitleBefore.Contains("doc."))
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

            // it was giving warning
            var x = result.ToArray();
            return x;
        }

        public void LoadFromCsv(FileInfo csvFile)
        {
            var lines = File.ReadAllLines(csvFile.FullName);

            lines = lines[1..];  // skip first line with field names

            foreach (var line in lines)
            {
                var s = line.Split(";");
                Add(new Person(s[0], s[1], s[2], s[3], s[4], s[5]));
            }
        }

        public void SaveToCsv(FileInfo csvFile)
        {
            var lines = new List<string>();

            foreach (var p in _people)
            {
                lines.Add(p.ToCsv());
            }

            File.WriteAllLines(csvFile.FullName, lines);
        }

        public void CopyTo(Person[] array, int arrayIndex)
        {
            _people.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Person> GetEnumerator() { return _people.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}
