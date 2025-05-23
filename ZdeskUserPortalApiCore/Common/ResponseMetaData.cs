﻿using System.Net;

namespace ZdeskUserPortalApiCore.Common
{
    public class ResponseMetaData<T>
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public bool IsError { get; set; }
        public string? ErrorDetails { get; set; }
        public string? CorrealtionId { get; set; }
        public T? Result { get; set; }
    }
}
