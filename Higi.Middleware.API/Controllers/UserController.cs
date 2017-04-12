using Higi.Middleware.Common;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Higi.Middleware.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private QueueClient queueClient;

        public UserController()
        {
            this.queueClient = QueueClient.CreateFromConnectionString(
                CloudConfigurationManager.GetSetting("ServiceBusConnectionString"));
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateHealthData(User user)
        {
            var message = JsonConvert.SerializeObject(user);

            await this.queueClient.SendAsync(new BrokeredMessage(message));

            return Ok();
        }
    }
}
