public class Expense : ICloneable{
    public decimal value{ get; set; }
    public string note { get; set; } = "";
    public string status { get; set; } = "pending";
    public int empid { get; set; }


    public object Clone(){
        return MemberwiseClone();
    }
}