using PedrosCantinaLib.Model;
using PedrosCantinaLib.Repository;

DBConnection db = new DBConnection();
EmployeeRepository employeeRepo = new EmployeeRepository();

bool isLeader = false;

while (true)
{
    db.ConnectToDB();
    Console.WriteLine("Velkommen \n");

    Console.WriteLine("1. Vis oversigt over alle medarbejdere: ");
    Console.WriteLine("2. Opret ny medarbejder: ");
    Console.WriteLine("3. Opdater oplysninger på en medarbejder: ");
    Console.WriteLine("4. Slet en medarbejder: ");

    Console.WriteLine("\n0. Afslut");

    string input = Console.ReadLine();

    // Show list of all Employees
    if (input == "1")
    {
        List<Employee> Employees = employeeRepo.ReadAll();
        foreach (Employee e in Employees)
        {
            Console.WriteLine($"[{e.EmployeeId}]: {e.Name} - {e.Phone} - {e.Email} - {e.Address} - {e.IsLeader}");
        }
        Console.WriteLine();


    }
    else if (input == "2")
    {

        Console.WriteLine("Indtast navn: ");
        string name = Console.ReadLine();

        Console.WriteLine("Indtast tlf: ");
        string phone = Console.ReadLine();

        Console.WriteLine("Indtast email: ");
        string email = Console.ReadLine();

        Console.WriteLine("Indtast adresse: ");
        string address = Console.ReadLine();

        Console.WriteLine("Er den nye ansat som leder? Angiv ja eller nej");
        string isLeaderChoice = Console.ReadLine();
        if (isLeaderChoice == "ja")
        {
            isLeader = true;

        }
        else if (isLeaderChoice == "nej")
        {
            isLeader = false;

        }

        Employee newEmployee = new Employee(0, name, phone, email, address, isLeader);
        Employee created = employeeRepo.Create(newEmployee);
        Console.WriteLine($"Medarbejder [{created.EmployeeId}] {name} oprettet!");
    }

    // Update Employee
    else if (input == "3")
    {
        Console.WriteLine("Angiv ID på medarbejder der skal ændres: ");
        int id = int.Parse(Console.ReadLine());

        Employee currentEmployee = employeeRepo.GetById(id);

        Console.WriteLine($"Nuværende oplysninger på {currentEmployee}:");
        Console.WriteLine("Tryk Enter for at beholde nuværende værdi: \n");

        Console.WriteLine($"Indtast navn ({currentEmployee.Name}): ");
        string name = Console.ReadLine();
        if (name == "") name = currentEmployee.Name;

        Console.WriteLine($"Indtast tlf ({currentEmployee.Phone}): ");
        string phone = Console.ReadLine();
        if (phone == "") phone = currentEmployee.Phone;

        Console.WriteLine($"Indtast email ({currentEmployee.Email}): ");
        string email = Console.ReadLine();
        if (email == "") email = currentEmployee.Email;

        Console.WriteLine($"Indtast adresse ({currentEmployee.Address}): ");
        string address = Console.ReadLine();
        if (address == "") address = currentEmployee.Address;

        bool newIsLeader = currentEmployee.IsLeader;
        while (true)
        {
            Console.WriteLine($"Skal medarbejderen være leder? Angiv ja eller nej ({(currentEmployee.IsLeader ? "ja" : "nej")}): ");
            string isLeaderChoice = Console.ReadLine()?.Trim().ToLower();

            if (isLeaderChoice == "") break;
            else if (isLeaderChoice == "ja") { newIsLeader = true; break; }
            else if (isLeaderChoice == "nej") { newIsLeader = false; break; }

            Console.WriteLine("Ugyldigt svar, skriv ja eller nej.");

        }
        Employee updatedEmployee = new Employee(id, name, phone, email, address, newIsLeader);
        Employee updated = employeeRepo.Update(id, updatedEmployee);

        Console.WriteLine($"\nMedarbejder [{updated.EmployeeId}] {updated.Name} opdateret!\n");
    }

    // Delete Employee
    else if (input == "4")
    {
        Console.WriteLine("Angiv ID på den medarbejder der skal slettes: ");
        int id = int.Parse(Console.ReadLine());
        Employee DeleteEmployee = employeeRepo.GetById(id);
        employeeRepo.Delete(id);

        Console.WriteLine($"Medarbejder [{DeleteEmployee.EmployeeId}] {DeleteEmployee.Name} er fyret." );

    }


    else if (input == "0")
    {
        break;
    }
}
