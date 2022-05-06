namespace Asp.Net_Core_WebApi.Models
{
    public class ReturnCode
    {
        //http标准
        public const int Failed = 400;
        public const int Empty = 204;
        public const int OK = 200;
        public const int Accepted = 202;


        //自定义
        public const int Error = -1;
    }
}
