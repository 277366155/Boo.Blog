using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using System.Net;

namespace AliOSS.WebApi.Service
{
    public class AliCloudOSSService
    {
        private readonly AliCloudOSSOptions _options;
        readonly OssClient _client;
        private const string _objectBaseName = "td/";
        public AliCloudOSSService(AliCloudOSSOptions options, OssClient client)
        {
            _options = options;
            _client = client;
        }

        #region 简单上传，单文件小于5GB
        /// <summary>
        /// 通过文件流上传
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SimpleUploads(Stream context, string fileName)
        {
            var isExist = _client.DoesObjectExist(_options.BucketName, GetObjectKey(fileName));
            if (isExist)
            {
                return "文件名重复，请重命名。";
            }
            var objMetadata = new ObjectMetadata { ContentType = HttpUtils.GetContentType(GetObjectKey(fileName), fileName) };
            var result = _client.PutObject(_options.BucketName, GetObjectKey(fileName), context, objMetadata);
            return result.ETag;
        }

        /// <summary>
        /// 从程序本地上传文件
        /// </summary>
        /// <param name="localFileName">文件路径</param>
        /// <returns></returns>
        public string SimpleUploads(string localFileName)
        {
            var isExist = _client.DoesObjectExist(_options.BucketName, GetObjectKey(localFileName));
            if (isExist)
            {
                return "文件名重复，请重命名。";
            }
            var result = _client.PutObject(_options.BucketName, GetObjectKey(localFileName), localFileName);
            return result.ETag;
        }
        #endregion

        #region 简单下载
        public async Task<Stream> SimpleDownloadsAsync(string fileName)
        {
            var exist = _client.DoesObjectExist(_options.BucketName, GetObjectKey(fileName));
            if (exist)
            {
                var result = _client.GetObject(_options.BucketName, GetObjectKey(fileName));
                return result.Content;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 删除文件
        public bool DeleteObject(string fileName)
        {
            var exist= _client.DoesObjectExist(_options.BucketName, GetObjectKey(fileName));
            if (exist)
            {
                var result = _client.DeleteObject(_options.BucketName, GetObjectKey(fileName));
                return result.HttpStatusCode == HttpStatusCode.NoContent;
            }
            else {
                return false;
            }
        }
        #endregion
        #region 检查文件是否存在
        public bool DoesObjectExist(string fileName)
        {
           return _client.DoesObjectExist(_options.BucketName,GetObjectKey(fileName)); 
        }
        #endregion

        /// <summary>
        /// 获取存储对象文件名
        /// </summary>
        /// <param name="localFileName">上传文件名</param>
        /// <param name="objectBaseName">oss中的目录位置</param>
        /// <returns></returns>
        private string GetObjectKey(string localFileName, string objectBaseName = _objectBaseName)
        {
            return objectBaseName + localFileName;
        }
    }
}