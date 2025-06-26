namespace Utilities
{
    public class ResponseModel<T> : IResponseModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}