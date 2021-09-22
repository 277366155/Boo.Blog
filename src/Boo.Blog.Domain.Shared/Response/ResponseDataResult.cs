namespace Boo.Blog.Response
{
    public class ResponseDataResult<T> : ResponseResult
    {
        public T Data { get; set; }     
    }
}
