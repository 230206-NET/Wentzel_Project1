using System.Data.SqlClient;
using Serilog;
public class DatabaseRepo
{
    SqlConnection conn;
    public DatabaseRepo()
    {
        conn = new SqlConnection(Secrets.secret);

    }

    public List<string> getPendingExpenses(){
        Log.Information("Getting pending expenses");
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM Expenses JOIN Employees ON EmployeeId = Employees.Id WHERE ExpenseType = 'pending'",conn);
        SqlDataReader reader = cmd.ExecuteReader();
        List<string> ret = new List<string>();
        ret.Add("Id\tValue\tNote\tName");
        while(reader.Read()){
            ret.Add(reader["Id"].ToString()+'\t'+reader["ExpenseValue"].ToString()+'\t'+reader["ExpenseNote"].ToString()+'\t'+reader["EmployeeName"].ToString());
        }
        conn.Close();
        return ret;
    }

    public List<string> getExpensesByEmpId(int id){
        Log.Information("Getting expenses for id {0}", id);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM Expenses JOIN Employees ON EmployeeId = Employees.Id WHERE Employees.Id = @id",conn);
        cmd.Parameters.AddWithValue("@id", id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<string> ret = new List<string>();
        ret.Add("Id\tValue\tNote\tType");
        while(reader.Read()){
            ret.Add(reader["Id"].ToString()+'\t'+reader["ExpenseValue"].ToString()+'\t'+reader["ExpenseNote"].ToString()+'\t'+reader["ExpenseType"].ToString());
        }
        conn.Close();
        return ret;
    }

    public void putNewExpense(string note, int empid, decimal value){
        Log.Information("Putting expense for employee {0}", empid);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT into Expenses(ExpenseValue, ExpenseNote, EmployeeId, ExpenseType) VALUES (@value, @note, @empid, 'pending');",conn);
        cmd.Parameters.AddWithValue("@value", value);
        cmd.Parameters.AddWithValue("@note", note);
        cmd.Parameters.AddWithValue("@empid", empid);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public string getPassByEmpId(string id){
        Log.Information("Getting password for employee {0}", id);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT EmployeePass FROM Employees WHERE Id = @id",conn);
        cmd.Parameters.AddWithValue("@id", id);

        string ret = (string)cmd.ExecuteScalar();
        conn.Close();
        return ret;
    }

    public int getEmployeeTypeById(string id){
        Log.Information("Getting employee type for id {0}", id);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT EmployeeType FROM Employees WHERE Id = @id",conn);
        cmd.Parameters.AddWithValue("@id", id);
        int ret = (int)cmd.ExecuteScalar();
        conn.Close();
        return ret;
    }

    public int newEmployee(string name, string pass){
        Log.Information("Making new employee with name {0} and password {1}",name, pass);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT into Employees(EmployeeType, EmployeeName, EmployeePass) OUTPUT INSERTED.Id VALUES (@type, @name, @pass);",conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@type", 1);
        cmd.Parameters.AddWithValue("@pass", pass);
        int ret = (int)cmd.ExecuteScalar();
        conn.Close();
        return ret;
    }

    public void setExpenseStatus(int id, string stat){
        Log.Information("Setting expense {0} status to {1}", id, stat);
        conn.Open();
        SqlCommand cmd = new SqlCommand("UPDATE Expenses SET ExpenseType = @status WHERE Id = @id",conn);
        cmd.Parameters.AddWithValue("@status", stat);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

}