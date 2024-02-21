namespace Grooveyard.Domain.Entities
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public T Data { get; set; }
    }

}
