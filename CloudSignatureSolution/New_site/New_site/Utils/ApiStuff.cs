using System;

namespace New_site.Utils
{

    public class ApiResponse : IApiResponse
    {
        public bool Success { get; }
        public bool Notify { get; }
        public string FriendlyMessageRo { get; }
        public string FriendlyMessageEn { get; }
        public ApiResponseSeverity Severity { get; }
        public object Data { get; }

        public ApiResponse(object data, string friendlyMessageRo = "", string friendlyMessageEn = "", ApiResponseSeverity severity = ApiResponseSeverity.Info, bool notify = false, bool success = true)
        {
            Data = data;
            FriendlyMessageEn = friendlyMessageEn;
            FriendlyMessageRo = friendlyMessageRo;
            Severity = severity;
            Success = success;
            Notify = notify;
        }
    }

    public interface IApiResponse
    {
        object Data { get; }
        string FriendlyMessageEn { get; }
        string FriendlyMessageRo { get; }
        bool Notify { get; }
        ApiResponseSeverity Severity { get; }
        bool Success { get; }
    }

    public enum ApiResponseSeverity
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
    }
}