using System;

namespace Boo.Blog.Response
{
    public class ResponseResult
    {
        public ResponseResultCode Code { get; set; }
        public string Message { get; set; }
        public long TimeStamp => (DateTime.UtcNow.Ticks - BlogConsts.BaseTicks) / 10000;

        public static ResponseResult IsSucess(string msg = "Succeed")
        {
            return new ResponseResult()
            {
                Code = ResponseResultCode.Succeed,
                Message = msg
            };
        }

        public static ResponseResult IsFail(string msg = "")
        {
            return new ResponseResult()
            {
                Code = ResponseResultCode.Fail,
                Message = msg
            };
        }

        public static ResponseResult IsFail(Exception ex)
        {
            return new ResponseResult()
            {
                Code = ResponseResultCode.Fail,
                Message = ex.InnerException?.StackTrace
            };
        }
    }
}
