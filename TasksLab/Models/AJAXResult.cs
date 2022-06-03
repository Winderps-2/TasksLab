namespace TasksLab.Models
{
    public class AJAXResult
    {
        public bool success; 
        public string message;

        public AJAXResult(bool _success, string _message)
        {
            success = _success;
            message = _message;
        }
    }
}