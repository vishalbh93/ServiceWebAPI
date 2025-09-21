namespace Service.Models
{
    public class SaveReturn<T>
    {
        public bool IsError { get; set; }

        public string? ErrorMessage { get; set; }

        public List<T>? ReturnResult { get; set; }
    }
}
