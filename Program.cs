
public class LoginManager{
    public void login(string username, string password){
        //todo
        //probably returns a session?
    }

    public void register(string username, string password){
        //todo
    }

}

public class localStorage{
    private static string _filePath = "localstore.json";
    FileStream fs;

    public localStorage()
    {
        fs = File.Open(_filePath, FileMode.OpenOrCreate);
    }

    public object getRecord(string key){
        //todo
        return null;
    }

    public void putRecord(string key, object obj){
        //todo
    }

}

public class UserHandler{
    enum userType
    {
        employee,
        manager
    }

    public void submitTicket(object obj){
        //use putrecord to submit ticket, validate input
        //maybe add return for success/failure? probably better to log
    }

    public List<object> getPending(){
        //get list of pending tickets, only usable by manager
        return null;
    }

    public List<object> getPrev(){
        //get previous submissions for employee
        return null;
    }

    public void processTicket(object obj){
        //allow manager to approve/deny
    }



}