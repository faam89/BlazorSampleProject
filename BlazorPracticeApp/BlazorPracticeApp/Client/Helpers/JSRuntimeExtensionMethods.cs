using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorPracticeApp.Client.Helpers
{
    public static class JSRuntimeExtensionMethods
    {
        
        public static async ValueTask<bool> Confirm(this IJSRuntime jS, string message)
        {
            await jS.InvokeVoidAsync("console.log", "Hi Amin", "This Is Test");
            return await jS.InvokeAsync<bool>("confirm", message);
        }

        public static async ValueTask ShowMessage(this IJSRuntime jS, string message)
        {
             await jS.InvokeVoidAsync("show_message", message);
        }
    }
}
