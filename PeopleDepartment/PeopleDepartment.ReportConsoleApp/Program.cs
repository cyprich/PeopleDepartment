using PeopleDepartment.CommonLibrary;


internal class Program
{
    public static string? InputArg { get; }
    public static string? OutputArg { get; }
    public static string? TemplateArg { get; }

    public static void Main(string[] args)
    {
        PersonCollection allPeople = new();
        allPeople.LoadFromCSV(new FileInfo("people-fri.csv"));
        //Console.WriteLine(allPeople.ToString());

        var r = allPeople.GenerateDepartmentReports();

        foreach (var x in r)
        {
            Console.WriteLine(x.Department);
            Console.WriteLine(x.Head);
            Console.WriteLine(x.Deputy);
            Console.WriteLine(x.Secretary);
            Console.WriteLine();
        }

    }

    public static void ParseArgs(string[] args)
    {

    }

    public static void Output()
    {

    }
}