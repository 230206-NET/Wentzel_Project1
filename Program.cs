using System.Data;
using System.Data.SqlClient;
using Serilog;

public class runner{
    static DatabaseRepo db = new DatabaseRepo();
    static int emptype = 0;
    static int empid;
    
    public static void Main(string[] args){
    Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/expenses.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        while (emptype == 0)
        {
            Console.WriteLine("Would you like to login (1) or register (2)?");
            switch(Console.ReadLine())
            {
                case "1":
                    login();
                    break;
                case "2":
                    register();
                    break;

            } 
        }
        switch(emptype){
            case 1:
                employeeMenu();
                break;
            case 2:
                managerMenu();
                break;
        }

    }
    public static void register(){
        Console.WriteLine("Enter a name:");
        string name = Console.ReadLine();
        Console.WriteLine("Enter a password:");
        string pass = Console.ReadLine();
        empid = db.newEmployee(name, pass);
        emptype = 1;
        Console.WriteLine("Your id is {0}", empid);
    }

    public static void login(){
        Console.WriteLine("Enter your Employee ID:");
        string id = Console.ReadLine();
        string pass = db.getPassByEmpId(id);
        if(String.IsNullOrEmpty(pass)){
            Console.WriteLine("It appears the id you entered does not exist, please register");
            return;
        }
        Console.WriteLine("Please enter your password:");
        if(Console.ReadLine().Equals(pass)){
            emptype = db.getEmployeeTypeById(id);
            empid = int.Parse(id);
        }else{
            Console.WriteLine("Your password did not match");
        }
    }

    public static void submitTicket(){
        string note;
        decimal value;
        while (true)
        {
            Console.WriteLine("Enter expense note:");
            note = Console.ReadLine();
            if(!String.IsNullOrEmpty(note)) break;
        }
        Console.WriteLine("Enter expense value:");
        value = decimal.Parse(Console.ReadLine());

        db.putNewExpense(note, empid, value);
    }

    public static void processTicket(){
        Console.WriteLine("Enter ticket ID you wish to process:");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Do you wish to (1) Approve, (2) Deny or (3) make Pending?");
        string input = Console.ReadLine();
        string type;
        switch(input){
            case "1":
                type = "approved";
                break;
            case "2":
                type = "denied";
                break;
            default:
                type = "pending";
                break;
        }

        db.setExpenseStatus(id, type);
    }

    public static void viewPending(){
        foreach(string s in db.getPendingExpenses()){
            Console.WriteLine(s);
        }
    }

    public static void viewPrevious(){
        foreach(string s in db.getExpensesByEmpId(empid)){
            Console.WriteLine(s);
        }
    }

    public static void employeeMenu(){
        while (true)
        {
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("[1] Submit ticket");
            Console.WriteLine("[2] View tickets");
            Console.WriteLine("[3] Exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    submitTicket();
                    break;
                case "2":
                    viewPrevious();
                    break;
                case "3":
                    return;
                    break;
            }
        }

    }

    public static void managerMenu(){
         while (true)
        {
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("[1] View pending tickets");
            Console.WriteLine("[2] Set ticket status");
            Console.WriteLine("[3] Exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    viewPending();
                    break;
                case "2":
                    processTicket();
                    break;
                case "3":
                    return;
            }
        }
    }


}