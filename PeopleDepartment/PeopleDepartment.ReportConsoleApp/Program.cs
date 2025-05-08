using PeopleDepartment.CommonLibrary;


internal class Program
{
    public static string InputArg { get; set; } = "";
    public static string? OutputArg { get; set; }
    public static string TemplateArg { get; set; } = "";

    public static void Main(string[] args)
    {
        ParseArgs(args);

        PersonCollection allPeople = new();
        allPeople.LoadFromCSV(new FileInfo("people-fri.csv"));
        //allPeople.saveToCSV(new FileInfo("pokus.csv"));
        var reports = allPeople.GenerateDepartmentReports();

        Output(reports);
    }

    public static void ParseArgs(string[] args)
    {
        // checking if required arguments are present
        if (!args.Contains("--input") || !args.Contains("--template"))
        {
            throw new ArgumentException("One of required parameters was not provided. " +
                                        "Required parameters: '--input' & '--template'");
        }

        // parsing "input"
        int inputIndex = Array.IndexOf(args, "--input") + 1;

        if (!File.Exists(args[inputIndex]))
        {
            throw new FileNotFoundException($"File \"{args[inputIndex]}\" not found");
        }

        InputArg = args[Array.IndexOf(args, "--input") + 1];

        // parsing "template"
        int templateIndex = Array.IndexOf(args, "--template") + 1;

        if (!File.Exists(args[templateIndex]))
        {
            throw new FileNotFoundException($"File \"{args[templateIndex]}\" not found");
        } 

        foreach (var line in File.ReadAllLines(args[templateIndex]))
        {
            TemplateArg += $"{line}\n";
        }

        // parsing "output"
        if (args.Contains("--output"))
        {
            int outputIndex = Array.IndexOf(args, "--output") + 1;

            if (!File.Exists(args[outputIndex]))
            {
                throw new FileNotFoundException($"File \"{args[outputIndex]}\" not found");
            }
            OutputArg = args[Array.IndexOf(args, "--output") + 1];
        }
    }

    public static void Output(DepartmentReport[] reports)
    {
        const string fallback = "<<Unknown>>";

        foreach (var r in reports)
        {
            var reportResult = TemplateArg;
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

            if (OutputArg == null)
            {
                Console.WriteLine(reportResult);
            }
            else
            {
                if (!Directory.Exists(OutputArg)) { Directory.CreateDirectory(OutputArg); }
                File.WriteAllLines($"{OutputArg}/{r.Department}.txt", reportResult.Split("\n"));
            }
        }
    }
}