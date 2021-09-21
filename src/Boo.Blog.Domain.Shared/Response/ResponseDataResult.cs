namespace Boo.Blog.Response
{
    public class ResponseDataResult<T> : ResponseResult
    {
        public T Data { get; set; }

        public static ResponseDataResult<T> IsSuccess(T data, string msg = "Succeed")
        {
            return new ResponseDataResult<T>
            {
                Data = data,
                Message = msg,
                Code = ResponseResultCode.Succeed
            };
        }     
    }
}
