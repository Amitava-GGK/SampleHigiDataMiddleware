using CPS.UI.Hubs;
using Higi.Middleware.Common;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CPS.UI.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        public IHttpActionResult HealthDataUpdateNotification(UserHealthData userHealthData)
        {
            if (userHealthData != null)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                var data = JsonConvert.SerializeObject(userHealthData);

                hubContext.Clients.All.userDataUpdateNotification(data);

                return Ok();
            }
            else
            {
                return BadRequest("UserHealthData can not be null");
            }
        }
    }
}
