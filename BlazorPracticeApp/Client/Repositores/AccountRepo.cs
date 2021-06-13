using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPracticeApp.Client.Helpers;
using BlazorPracticeApp.Shared.DTOs;

namespace BlazorPracticeApp.Client.Repositores
{
    public class AccountRepo : IAccountRepo
    {
        private readonly IHttpService httpService;
        private readonly string url = "api/Account";

        public AccountRepo(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<UserToken> Register(UserInfo userInfo)
        {
            var response = await httpService.Post<UserInfo, UserToken>($"{url}/Create", userInfo);
            if (!response.IsSuccess)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }

        public async Task<UserToken> Login(UserInfo userInfo)
        {
            var response = await httpService.Post<UserInfo, UserToken>($"{url}/Login", userInfo);

            if (!response.IsSuccess)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }
    }
}
