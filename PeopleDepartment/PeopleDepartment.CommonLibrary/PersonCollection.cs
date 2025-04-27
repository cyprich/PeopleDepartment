using System.Collections.Specialized;

namespace PeopleDepartment.CommonLibrary
{
    internal class PersonCollection
    {
        public int Count { get; }
        public bool IsReadOnly { get; }
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        // TODO implement these

        //public void Add(Person person) { }

        //public bool Remove(Person person) { }

        //public IEnumerator<Person> GetEnumerator() { }

        //public void LoadFromCSV(FileInfo csvFile) { }

        //public void saveToCSV(FileInfo csvFile) { }

        //public DepartmentReport[] GenerateDepartmentReports() { }

        //public void Clear() { }

        //public bool Contains(Person person) { }

        //public void CopyTo(Person[] array, int arrayIndex) { }
    }
}
