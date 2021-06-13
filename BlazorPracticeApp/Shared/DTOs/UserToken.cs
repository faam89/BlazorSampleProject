using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorPracticeApp.Shared.DTOs
{
   public class UserToken
    {
        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
