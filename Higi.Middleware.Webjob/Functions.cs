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

namespace Higi.Middleware.Webjob
{
    public class Functions
    {
        private static HttpClient httpClient = new HttpClient();

        private static readonly string higiApiEndPoint = ConfigurationManager.AppSettings["HigiApiEndpoint"];

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static async Task ProcessQueueMessage([ServiceBusTrigger("higidemoqueue")] string message, TextWriter log)
        {
            
            //log.WriteLine(message);

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

            }

        }
    }
}
