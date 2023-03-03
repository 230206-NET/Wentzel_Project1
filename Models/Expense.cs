namespace Models;
public class Expense{
    public decimal value{ get; set; }
    public string note { get; set; } = "";
    public string status { get; set; } = "";
    public int id { get; set; }
    public int empid {get; set;}

}