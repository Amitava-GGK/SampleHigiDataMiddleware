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
        public static async Task ProcessQueueMessage([ServiceBusTrigger("hmwqueue")] string message, TextWriter log)
        {

            //log.WriteLine(message);

            Console.WriteLine("Received message from service bus: {0}", message);

            UserDataUpdateMessage userDataUpdateMessage = JsonConvert.DeserializeObject<UserDataUpdateMessage>(message);

            using (var dbContext = new SampleHigiMiddlewareDBEntities())
            {
                var queueMessage = await dbContext.QueueMessageStatus.FindAsync(userDataUpdateMessage.MessageId);
                queueMessage.Status = UserDataUpdateStatus.Processing;

                await dbContext.SaveChangesAsync();
            }
            
            //Make request to higi api to get user health data

            UserHealthData userHealthData = await GetUserDataFromHigi(userDataUpdateMessage);
            

            if (userHealthData != null)
            {
                //Todo: Make call to client endpoint based on user id

                IEnumerable<Target> targets;

                using (var dbContext = new SampleHigiMiddlewareDBEntities())
                {
                    targets = dbContext.Higi_Client_Mappings
                        .Where(m => m.HigiUserId == userDataUpdateMessage.UserId)
                        .Select(u => u.Target)
                        .ToList();
                }
                
                foreach (var target in targets)
                {

                    var content = new StringContent(JsonConvert.SerializeObject(userHealthData), Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = new Uri(target.Endpoint),
                        Content = content
                    };

                    var formattedRequest = await FormatRequestMessage(request);

                    var clientResponse = await httpClient.SendAsync(request);

                    using (var dbContext = new SampleHigiMiddlewareDBEntities())
                    {
                        var clientRequestResponse = new ClientRequestResponse
                        {
                            ClientId = target.ClientId,
                            MessageId = userDataUpdateMessage.MessageId,
                            Request = formattedRequest,
                            Response = await FormatResponseMessage(clientResponse),
                            status = clientResponse.ReasonPhrase,
                            DateTimeCreated = DateTime.Now
                        };

                        dbContext.ClientRequestResponses.Add(clientRequestResponse);

                        var queuemessage = await dbContext.QueueMessageStatus.FindAsync(userDataUpdateMessage.MessageId);
                        queuemessage.Status = clientResponse.IsSuccessStatusCode ? UserDataUpdateStatus.ClientApiCallSuccess : UserDataUpdateStatus.ClientApiCallError;

                        await dbContext.SaveChangesAsync();
                    }

                    if (clientResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Successfully called client endpoint. \r\nEndpoint: {0}", target.Endpoint);
                    }
                    else
                    {
                        Console.WriteLine("Failed to call client endpoint.\r\nEndpoint: {0}", target.Endpoint);
                    }
                }
            }

        }

        private static async Task<UserHealthData> GetUserDataFromHigi(UserDataUpdateMessage userDataUpdateMessage)
        {
            //Make request to higi api to get user health data

            UserHealthData userHealthData = null;

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{higiApiEndPoint}?userId={userDataUpdateMessage.UserId}")
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);

            using (var dbContext = new SampleHigiMiddlewareDBEntities())
            {
                var higiRequestResponse = new HigiRequestResponse
                {
                    MessageId = userDataUpdateMessage.MessageId,
                    Request = await FormatRequestMessage(request),
                    Response = await FormatResponseMessage(response),
                    status = response.ReasonPhrase,
                    DateTimeCreated = DateTime.Now
                };

                dbContext.HigiRequestResponses.Add(higiRequestResponse);

                var queuemessage = await dbContext.QueueMessageStatus.FindAsync(userDataUpdateMessage.MessageId);
                queuemessage.Status = response.IsSuccessStatusCode ? UserDataUpdateStatus.HigiApiCallSuccess : UserDataUpdateStatus.HigiApiCallError;

                await dbContext.SaveChangesAsync();
            }

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

            return userHealthData;
        }

        private static async Task<string> FormatRequestMessage(HttpRequestMessage request)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{request.Method.ToString()} {request.RequestUri.ToString()} HTTP/{request.Version}");
            sb.AppendLine($"{request.Headers.ToString()}");

            if (request.Content != null)
            {
                try
                {
                    sb.AppendLine($"{Encoding.UTF8.GetString(await request.Content.ReadAsByteArrayAsync())}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return sb.ToString();
        }

        private static async Task<string> FormatResponseMessage(HttpResponseMessage response)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"HTTP/{response.Version} {Convert.ToInt32(response.StatusCode)} {response.ReasonPhrase}");
            sb.AppendLine($"{response.Headers.ToString()}");

            if (response.Content != null)
            {
                sb.AppendLine($"{Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync())}");
            }

            return sb.ToString();
        }
    }
}
