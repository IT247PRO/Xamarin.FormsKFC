using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int user_Id { get; set; }
        public string user_name { get; set; }
        public string expires_in { get; set; }
        public int creating_Time { get; set; }
        public int expiration_Time { get; set; }
    }
}
