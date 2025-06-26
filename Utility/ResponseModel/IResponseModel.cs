namespace Utilities
{
    public interface IResponseModel<T>
    {
        T Data { get; set; }
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}