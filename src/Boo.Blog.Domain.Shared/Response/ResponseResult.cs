using Boo.Blog.Consts;
using System;

namespace Boo.Blog.Response
{
    public class ResponseResult
    {
        public ResponseResultCode Code { get; set; }
        public string Message { get; set; }
        public long TimeStamp => (DateTime.UtcNow.Ticks - BlogConsts.BaseTicks) / 10000;

        #region 返回类型为ResponseResult
        public static ResponseResult IsSucess(string msg = "Succeed")
        {
            return new ResponseResult()
            {
                Code = ResponseResultCode.Succeed,
                Message = msg
            };
        }

        public static ResponseResult IsFail(string msg = "Fail")
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
                Message = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
            };
        }
        #endregion 返回类型为ResponseResult


        #region 返回类型为ResponseDataResult<T>
        public static ResponseDataResult<T> IsFail<T>(string msg = "Fail")
        {
            return new ResponseDataResult<T>()
            {
                Code = ResponseResultCode.Fail,
                Message = msg
            };
        }

        public static ResponseDataResult<T> IsFail<T>(Exception ex)
        {
            return new ResponseDataResult<T>()
            {
                Code = ResponseResultCode.Fail,
                Message = ex.InnerException==null?ex.StackTrace:ex.InnerException.StackTrace
            };
        }

        public static ResponseDataResult<T> IsSuccess<T>(T data, string msg = "Succeed")
        {
            return new ResponseDataResult<T>
            {
                Data = data,
                Message = msg,
                Code = ResponseResultCode.Succeed
            };
        }

        #endregion 返回类型为ResponseDataResult<T>
    }
}
