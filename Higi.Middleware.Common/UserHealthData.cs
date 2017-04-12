using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higi.Middleware.Common
{
    public class UserHealthData
    {
        public int UserId { get; set; }

        public Dictionary<string, string> HealthData { get; set; }
    }
}
