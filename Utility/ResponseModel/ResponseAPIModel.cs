namespace Utilities
{
    public class ResponseAPIModel<T> : IResponseModel<T>
    {
        public string Code { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public T Data { get; set; }
    }
}
