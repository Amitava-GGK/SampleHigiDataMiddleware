using Higi.Middleware.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Higi.UI.Controllers
{
    public class UserController : ApiController
    {
        private List<UserHealthData> userHealthDataList;
        private static HttpClient httpClient = new HttpClient();

        public UserController()
        {
            this.userHealthDataList = new List<UserHealthData>
            {
                new UserHealthData
                {
                    UserId = 1,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "64kg"},
                        { "height", "5.8'" },
                        { "bmi", "2.4" }
                    }
                },
                new UserHealthData
                {
                    UserId = 2,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "96kg"},
                        { "height", "4.2'" },
                        { "bmi", "7.8" }
                    }
                },
                new UserHealthData
                {
                    UserId = 3,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "76kg"},
                        { "height", "6.1'" },
                        { "bmi", "3.6" }
                    }
                },
                new UserHealthData
                {
                    UserId = 4,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "82kg"},
                        { "height", "5.6'" },
                        { "bmi", "2.4" }
                    }
                }
            };
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserHealthData(User user)
        {
            if (user == null)
            {
                return BadRequest("User can not be null");
            }

            var middlewareEndpoint = ConfigurationManager.AppSettings["HigiMiddlewareEndpoint"];
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{middlewareEndpoint}/User/UpdateHealthData", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public IHttpActionResult GetUserHealthData(int userId)
        {
            var userHealthData = this.userHealthDataList.FirstOrDefault(u => u.UserId == userId);

            return Ok<UserHealthData>(userHealthData);
        }
    }
}
