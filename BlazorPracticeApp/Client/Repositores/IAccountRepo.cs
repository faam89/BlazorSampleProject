using System.Threading.Tasks;
using BlazorPracticeApp.Shared.DTOs;

namespace BlazorPracticeApp.Client.Repositores
{
    public interface IAccountRepo
    {
        Task<UserToken> Register(UserInfo userInfo);
        Task<UserToken> Login(UserInfo userInfo);
    }
}