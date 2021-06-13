using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorPracticeApp.Client.Auth
{
    public class TestAuthenticationStateProvider : AuthenticationStateProvider 
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonymous = new ClaimsIdentity(new List<Claim>
            {
                new Claim("key1","value1"),
                new Claim(ClaimTypes.Name,"AminFayazi"),
                new Claim(ClaimTypes.Role,"Admin"),
            },"test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
