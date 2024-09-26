using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepUp
{
    public static class UserSession
    {
        public static int CustomerID { get; set; } = 0; // 0 means no user is logged in
        public static string UserName { get; set; } = string.Empty;
    }
}
