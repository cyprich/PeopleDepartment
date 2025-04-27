using PeopleDepartment.CommonLibrary;


internal class Program
{
    public static string InputArg { get; } = "";
    public static string? OutputArg { get; }
    public static string TemplateArg { get; set; }

    public static void Main(string[] args)
    {
        PersonCollection allPeople = new();
        allPeople.LoadFromCSV(new FileInfo("people-fri.csv"));
        var reports = allPeople.GenerateDepartmentReports();

        ParseArgs(args);
        Output(reports);
    }

    public static void ParseArgs(string[] args)
    {
        // TODO
        foreach (var line in File.ReadAllLines("templ-dep-sk.txt"))
        {
            TemplateArg += $"{line}\n";
        }
    }

    public static void Output(DepartmentReport[] reports)
    {
        string result = "";
        string fallback = "<<Unknown>>";

        foreach (var r in reports)
        {
            string reportResult = TemplateArg;
            reportResult = reportResult.Replace("[[Department]]", r.Department);
            reportResult = reportResult.Replace("[[Head]]", r.Head?.ToString() ?? fallback);
            reportResult = reportResult.Replace("[[Deputy]]", r.Deputy?.ToString() ?? fallback);
            reportResult = reportResult.Replace("[[Secretary]]", r.Secretary?.ToString() ?? fallback);
            reportResult = reportResult.Replace("[[NumberOfEmployees]]", r.NumberOfEmployees.ToString());
            reportResult = reportResult.Replace("[[NumberOfProfessors]]", r.NumberOfProfessors.ToString());
            reportResult = reportResult.Replace("[[NumberOfAssociateProfessors]]", r.NumberOfAssociateProfessors.ToString());
            reportResult = reportResult.Replace("[[NumberOfPhDStudents]]", r.NumberOfPhDStudents.ToString());
            reportResult = reportResult.Replace("[[Employees]]", string.Join("\n", r.Employees.Select(e => e.ToString())));
            reportResult = reportResult.Replace("[[PhDStudents]]", string.Join("\n", r.PhDStudents.Select(s => s.ToString())));

            result += $"{reportResult}\n";
        }

        // TODO
        Console.WriteLine(result);  
    }
}