using System.Threading.Tasks;

namespace BlazorPracticeApp.Client.Auth
{
    public interface ILoginService
    {
        Task Login(string token);
        Task LogOut();
    }
}