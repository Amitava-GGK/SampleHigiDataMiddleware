using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higi.Middleware.Common
{
    public class UserDataUpdateMessage
    {
        public int UserId { get; set; }

        public int MessageId { get; set; }
    }
}
