using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Higi.Middleware.Common
{
    public class UserDataUpdateStatus
    {
        public const string MiddlewareReceived = "Received at middleware";
        public const string Queued = "Queued";
        public const string Processing = "Processing";
        public const string HigiApiCallError = "Higi Api call failed";
        public const string HigiApiCallSuccess = "Higi Api call succeed";
        public const string ClientApiCallError = "Client Api call failed";
        public const string ClientApiCallSuccess = "Client Api call succeed";
    }
}
