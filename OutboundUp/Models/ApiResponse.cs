namespace OutboundUp.Models
{
    public class ApiResponse<T> where T : class
    {
        public ApiResponse(T data, IEnumerable<string>? errors = null)
        {
            Data = data;
            Errors = errors;
        }

        public T Data { get; }
        public IEnumerable<string>? Errors { get; }
    }
}
