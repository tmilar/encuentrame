using System;
using System.Net;
using NailsFramework.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Encuentrame.Support.ExpoNotification
{
    public class ExpoPushHelper
    {
        [Inject("ExpoNotificationUrl")]
        public static string ExpoNotificationUrl { get; set; }
        public static void SendPushNotification(string expoToken, string title,string body, dynamic data)
        {
            dynamic bodySend = new
            {
                to = expoToken,
                title = "hello",
                body = "world",
                sound = "default",
                data = new { some = "daaaata" }
            };
            string response = null;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("accept", "application/json");
                client.Headers.Add("accept-encoding", "gzip, deflate");
                client.Headers.Add("Content-Type", "application/json");
                response = client.UploadString(ExpoNotificationUrl, JsonConvert.SerializeObject(bodySend));
            }
            dynamic json = JObject.Parse(response);
            
             
        }
    }
}