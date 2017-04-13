using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using Higi.Middleware.Common;
using Higi.Middleware.Data;

namespace Higi.Middleware.Webjob
{
    public class Functions
    {
        private static HttpClient httpClient = new HttpClient();

        private static readonly string higiApiEndPoint = ConfigurationManager.AppSettings["HigiApiEndpoint"];

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static async Task ProcessQueueMessage([ServiceBusTrigger("samplehigiqueue")] string message, TextWriter log)
        {

            //log.WriteLine(message);

            Console.WriteLine("Received message from service bus: {0}", message);

            User user = JsonConvert.DeserializeObject<User>(message);

            //Make request to higi api to get user health data

            UserHealthData userHealthData = null;
            HttpResponseMessage response = await httpClient.GetAsync($"{higiApiEndPoint}?userId={user.UserId}");

            if (response.IsSuccessStatusCode)
            {
                
                try
                {
                    userHealthData = await response.Content.ReadAsAsync<UserHealthData>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured while reading the response content.\r\n {0}", ex);
                }

                Console.WriteLine("Received user health data. \r\n {0}", JsonConvert.SerializeObject(userHealthData));
                
            }

            if (userHealthData != null)
            {
                //Todo: Make call to client endpoint based on user id

                using (var dbContext = new SampleHigiMiddlewareDBEntities())
                {
                    var userMappings = dbContext.Higi_Client_Mappings.Where(m => m.HigiUserId == user.UserId);

                    foreach (var mapping in userMappings)
                    {
                        var endpoint = mapping.Target.Endpoint;

                        var content = new StringContent(JsonConvert.SerializeObject(userHealthData), Encoding.UTF8, "application/json");

                        var clientResponse = await httpClient.PostAsync(endpoint, content);

                        if (clientResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Successfully called client endpoint. \r\nEndpoint: {0}", endpoint);
                        }
                        else
                        {
                            Console.WriteLine("Failed to call client endpoint.\r\nEndpoint: {0}", endpoint);
                        }
                    }
                }
            }

        }
    }
}
