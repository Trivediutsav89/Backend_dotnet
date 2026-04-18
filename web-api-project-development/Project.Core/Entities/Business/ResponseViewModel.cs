
namespace Project.Core.Entities.Business
{
    public interface IResponseViewModel
    {
        bool Success { get; set; }
        string? Message { get; set; }
        ErrorViewModel? Error { get; set; }
        string? RequestId { get; set; }
        DateTime Timestamp { get; set; }
    }

    public class ResponseViewModel<T> : IResponseViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public ErrorViewModel? Error { get; set; }
        public string? RequestId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ResponseViewModel : IResponseViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorViewModel? Error { get; set; }
        public string? RequestId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
