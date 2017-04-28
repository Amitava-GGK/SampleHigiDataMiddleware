using Higi.Middleware.Common;
using Higi.Middleware.Data;
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
            var userDataUpdateMessage = new UserDataUpdateMessage
            {
                UserId = user.UserId
            };

            using (var dbContext = new SampleHigiMiddlewareDBEntities())
            {
                var queueMessage = new QueueMessageStatu
                {
                    HigiUserId = user.UserId,
                    Status = UserDataUpdateStatus.MiddlewareReceived,
                    DateTimeCreated = DateTime.Now
                };

                dbContext.QueueMessageStatus.Add(queueMessage);
                await dbContext.SaveChangesAsync();

                userDataUpdateMessage.MessageId = queueMessage.MessageId;
            }

            var message = JsonConvert.SerializeObject(userDataUpdateMessage);

            await this.queueClient.SendAsync(new BrokeredMessage(message));

            using (var dbContext = new SampleHigiMiddlewareDBEntities())
            {
                var queueMessage = await dbContext.QueueMessageStatus.FindAsync(userDataUpdateMessage.MessageId);
                queueMessage.Status = UserDataUpdateStatus.Queued;

                await dbContext.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
