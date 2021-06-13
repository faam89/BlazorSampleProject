using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPracticeApp.Client.Helpers
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);

        Task<HttpResponseWrapper<object>> Put<T>(string url, T data);

        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data);

        Task<HttpResponseWrapper<T>> Get<T>(string url);

    }
}
