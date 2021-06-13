using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPracticeApp.Client.Helpers
{
    public static class IHttpServiceExtentionMethods
    {
        public static async Task<T> GetHelper<T>(this IHttpService _httpService,string url)
        {
            var response = await _httpService.Get<T>(url);
            if (!response.IsSuccess)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

    }
}
