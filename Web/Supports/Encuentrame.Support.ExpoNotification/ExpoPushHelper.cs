using System;
using System.Collections.Generic;
using System.Net;
using NailsFramework.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Encuentrame.Support.ExpoNotification
{
    public class ExpoPushHelper
    {
        [Inject("ExpoNotificationUrl")]
        public static string ExpoNotificationUrl { get; protected set; }


        public static void SendPushNotification(IList<BodySend> bodies)
        {
            if (bodies == null && bodies.Count == 0)
            {
                return;
            }
                
            var list=new List<Bodyinternal>();
            foreach (var body in bodies)
            {
                Bodyinternal bodySend = new Bodyinternal()
                {
                    to = body.Token,
                    title = body.Title,
                    body = body.Body,
                    sound = "default",
                    data = body.Data
                };
                list.Add(bodySend);
            }
           
            string response = null;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("accept", "application/json");
                client.Headers.Add("accept-encoding", "gzip, deflate");
                client.Headers.Add("Content-Type", "application/json");
                response = client.UploadString(ExpoNotificationUrl, JsonConvert.SerializeObject(list));
            }
            dynamic json = JObject.Parse(response);
            
             
        }

        public class Bodyinternal
        {
            public string to { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public string sound { get; set; }
            public dynamic data { get; set; }
        }
    }
}