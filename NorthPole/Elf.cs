using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace NorthPole
{
    public class Elf : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
                return;

            var userkey = context.Request.QueryString.Get("key");

            var secretkey = userkey + ConfigurationManager.AppSettings["secretkey"];

            var timestamp = DateTime.UtcNow.ToBinary();

            string input = string.Empty;
            using(var sr = new StreamReader(context.Request.InputStream))
            {
                input = sr.ReadToEnd();
            }

            input += string.Format("{0}{1}", Environment.NewLine, timestamp);

            var hmac = Rudolph.GenerateHMAC(input, secretkey);

            context.Response.Output.WriteLine(timestamp);
            context.Response.Output.WriteLine(hmac);
            context.Response.Output.Flush();            
        }
    }
}
